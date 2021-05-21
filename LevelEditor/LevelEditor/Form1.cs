using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace LevelEditor
{
    //Name: Sami Chamberlain
    //Date: 2/12/2021
    //Purpose: Directs the user to either a loaded map or
    //a new one.
    public partial class Form1 : Form
    {
        //fields
        private int height;
        private int width;
        private string message;
        private string[,] colors;
        private string[,] overlayColors;
        private string[,] collisionColors;
        private bool rotationsLoaded;
        private int[,] rotationValues;
        private int[,] overlayRotation;
        private int[,] collisionRotation;


        //Constructor

        /// <summary>
        /// Creates the initial menu form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            rotationsLoaded = false;


        }

        /// <summary>
        /// Loads a level for the user when they click the button
        /// </summary>
        /// <param name="sender">The button click</param>
        /// <param name="e"></param>
        private void loadButton_Click(object sender, EventArgs e)
        {
            //Order -
            //Width, Height, Color ARGB Values

            //Opens file explorer for the user, allows
            //them to choose a file
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open a level file";
            dialog.Filter = "Level Files|*.level";
            DialogResult r = dialog.ShowDialog();

            FileStream stream = null;
            BinaryReader reader = null;
            levelEditor editor = null;

            //User chooses OK in the file explorer
            if (r == DialogResult.OK)
            {

                try
                {
                    //Reads from the file
                    stream = new FileStream(dialog.FileName, FileMode.Open);

                    reader = new BinaryReader(stream);

                    //Width and height obtained
                    width = reader.ReadInt32();
                    height = reader.ReadInt32();

                    //establishes the level editor with the given information
                    editor = new levelEditor(width, height);


                    colors = new string[width, height];
                    collisionColors = new string[width, height];

                    rotationValues = new int[width, height];
                    collisionRotation = new int[width, height];

                    editor.RotationValues = rotationValues;
                    editor.CollisionValues = collisionRotation;



                    //BACKGROUND LAYER
                    editor.LoadColors(colors, reader, rotationValues, rotationsLoaded);
                    editor.LoadColors(collisionColors, reader, collisionRotation, rotationsLoaded);

                    
                    //loads the picture boxes and matches the colors
                    editor.LoadBoxes(colors, collisionColors);

                    //Properly sizes the form
                    editor.ResizeForm();

                    //Prompts the user that it was sucessful!
                    editor.Text = $"Level editor - { dialog.FileName.Remove(0, dialog.FileName.LastIndexOf('\\') + 1)}";
                    MessageBox.Show("Successfully loaded the file!", ":)");
                    stream.Close();
                    editor.ShowDialog();
                }

                catch (Exception ex)
                {
                    //Something was wrong with the file
                    MessageBox.Show("Error reading file! " + ex.Message, ":(");
                    stream.Close();
                }
            }

            //User exited out of the file explorer...
            else
            {
                return;
            }
        }

        /// <summary>
        /// Creates a fresh map for the user
        /// on request
        /// </summary>
        /// <param name="sender">Button press</param>
        /// <param name="e"></param>
        private void createButton_Click(object sender, EventArgs e)
        {
            //Prints errors, in case params are not 
            //entered correctly
            message = "Errors: \n";
            bool validWidth = int.TryParse(widthBox.Text, out width);
            bool validHeight = int.TryParse(heightBox.Text, out height);

            //width not valid
            if (validWidth == false)
            {
                message = message + " - Not a valid Width.\n";
            }

            //height not valid
            if (validHeight == false)
            {
                message = message + " - Not a valid Height.\n";
            }

            //too small width
            if (width < 10 && validWidth)
            {
                message = message + " - Width too small, the minimum is 10.\n";
            }

            //too large width
            if (width > 30 && validWidth)
            {
                message = message + " - Width too large, the maximum is 30.\n";
            }

            //too small height
            if (height < 10 && validHeight)
            {
                message = message + " - Height too small, the minimum is 10.\n";
            }

            //too large height
            if (height > 30 && validHeight)
            {
                message = message + " - Height to large, the maximum is 30.\n";
            }

            //prints errors to a message box, if any
            if (message != "Errors: \n")
            {
                MessageBox.Show(message, "Error creating level :(");
            }

            //No errors!
            else
            {
                //successfully creates a new level editor instance
                levelEditor editor = new levelEditor(width, height);

                //generates the boxes (previously caused an error when put in the 
                //initialize)
                editor.GenerateBoxes("../../../default-min.png");
                //shows the level
                editor.ShowDialog();
            }
        }

        /// <summary>
        /// Formats a .level file into a .level_Appended file, which
        /// removes the "../../../" from image paths
        /// These files are readable by our game, and store tile, Vector2, and
        /// Beginning rectangle data
        /// </summary>
        /// <param name="sender">Export button click</param>
        /// <param name="e">Handles events</param>
        private void exportButton_Click(object sender, EventArgs e)
        {
            //Opens a window to allow the user to
            //choose a file
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Convert a .level file...";
            dialog.Filter = "Level Files|*.level";
            DialogResult r = dialog.ShowDialog();

            FileStream stream = null;
            FileStream writeStream = null;
            BinaryReader reader = null;
            BinaryWriter writer = null;

            //User chooses OK in the file explorer
            if (r == DialogResult.OK)
            {

                try
                {
                    //Reads AND writes from the file
                    stream = new FileStream(dialog.FileName, FileMode.Open);
                    writeStream = new FileStream(dialog.FileName + "_Appended", FileMode.Create);
                    reader = new BinaryReader(stream);
                    writer = new BinaryWriter(writeStream);

                    //Width and height obtained
                    width = reader.ReadInt32();
                    writer.Write(width);
                    height = reader.ReadInt32();
                    writer.Write(height);

                    //Handles the background layer, along with it's rotation
                    //values
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height * 2; j++)
                        {
                            //gets the path
                            string currentPicture = reader.ReadString();
                            //gets the rotation value stored next to it
                            int rotationValue = reader.ReadInt32();

                            //Colors obtained (Vector2 data)
                            if (currentPicture.Contains("<"))
                            {
                                writer.Write(currentPicture);
                                writer.Write(rotationValue);
                                continue;
                            }
                            //Not a color, write the tile ID without the
                            //path
                            else if (currentPicture.Contains("../../../"))
                            {
                                currentPicture = currentPicture.Substring
                                    (currentPicture.LastIndexOf('/') + 1,
                                    currentPicture.LastIndexOf('.') - currentPicture.LastIndexOf('/') - 1);
                            }

                            //writes the altered picture path and rotation value
                            //to a new file
                            writer.Write(currentPicture);
                            writer.Write(rotationValue);
                            
                        }
                    }
                }

                catch (Exception ex)
                {
                    //Something was wrong with the file
                    MessageBox.Show("Error reading file! " + ex.Message, ":(");
                }

                finally
                {
                    //File was successfully appended!
                    if (stream != null)
                    {
                        MessageBox.Show("Successfully appended the file!");
                        reader.Close();
                    }
                }
            }
        }
    }
}
