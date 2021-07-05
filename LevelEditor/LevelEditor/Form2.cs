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
using MsgBox;

//Name: Sami Chamberlain
//Date: 2/13/2021
//Purpose: Allows a user to paint individual tiles that serve
//as the tiles of a level

//auth testt

namespace LevelEditor
{
    public partial class levelEditor : Form
    {
        //fields
        private int height;
        private int width;
        private int boxHeight;
        private int boxWidth;
        private PictureBox[,] boxes;
        private PictureBox[,] collisions;
        private PictureBox[,] objects;
        private String[,] metadataStorage;
        private List<Button> buttons;
        private FileStream stream;
        private string[,] colors;
        private string[,] collisionColors;
        private string[,] objectColors;
        private bool isSaved;
        private int rotationSaveX;
        private int rotationSaveY;
        private String metadata;

        private string firstFileName;
        private string firstFilePath;
        private int numSections;

        private Color currentColor;

        private string path;
        private int[,] rotationValues;
        private int[,] collisionRotations;
        private int[,] objectRotations;

        private int active;

        //ROTATION
        private float rotation;

        private int originalBoxHeight;
        private int originalBoxWidth;

        private bool biggerHeight;
        private bool biggerWidth;



        private int numLayersSaved;

        private bool prevButtonHit;

        //Properties

        /// <summary>
        /// Returns a list of rotation values
        /// or alters the list of rotation values
        /// </summary>
        public int[,] RotationValues
        {
            get { return rotationValues; }
            set { rotationValues = value; }
        }

        /// <summary>
        /// sets the colors list for the editor
        /// </summary>
        public string[,] Colors
        {
            set { colors = value; }
        }

        /// <summary>
        /// sets the collision colors of the
        /// editor
        /// </summary>
        public string[,] CollisionColors
        {
            set { collisionColors = value; }
        }

        /// <summary>
        /// Returns a list of rotation values
        /// or alters the list of rotation values
        /// </summary>
        public int[,] CollisionValues
        {
            get { return collisionRotations; }
            set { collisionRotations = value; }
        }

        /// <summary>
        /// sets the object colors
        /// </summary>
        public string[,] ObjectColors
        {
            set { objectColors = value; }
        }

        /// <summary>
        /// gets or sets the rotation values of the object layer
        /// </summary>
        public int[,] ObjectValues
        {
            get { return objectRotations; }
            set { objectRotations = value; }
        }

        /// <summary>
        /// sets the num sections of the editor
        /// </summary>
        public int Num
        {
            set { numSections = value; }
        }

        /// <summary>
        /// sets or gets the first file name
        /// </summary>
        public string FirstName
        {
            set { firstFileName = value; }
            get { return firstFileName; }
        }

        /// <summary>
        /// sets the first file path of the editor
        /// </summary>
        public string FirstPath
        {
            set { firstFilePath = value; }
        }

        /// <summary>
        /// Creates the level editor form
        /// </summary>
        /// <param name="width">The width of the level</param>
        /// <param name="height">The height of the level</param>
        public levelEditor(int width, int height)
        {
            InitializeComponent();

            this.height = height;
            this.width = width;

            boxes = new PictureBox[height, width];
            collisions = new PictureBox[height, width];
            objects = new PictureBox[height, width];
            metadataStorage = new String[height, width];
            rotationValues = new int[height, width];
            collisionRotations = new int[height, width];
            objectRotations = new int[height, width];  
            isSaved = true;
            numLayersSaved = 0;

            firstFileName = null;

            active = 1;

            buttons = new List<Button>();

            currentColor = Color.White;

            path = "../../../Default size/1.png";
            texturePic.Load(path);
            texturePic.SizeMode = PictureBoxSizeMode.Zoom;

            backgroundButton.BackColor = Color.Green;
            rotation = 0;
            rotationSaveX = 0;
            rotationSaveY = 0;
            numSections = 0;
            originalBoxHeight = 0;
            originalBoxWidth = 0;
            biggerHeight = false;
            biggerWidth = false;

            prevButtonHit = false;
        }

        /// <summary>
        /// Returns the Height of the level
        /// </summary>
        public int Height { get { return height; } }

        /// <summary>
        /// Returns the Width of the level
        /// </summary>
        public int Width { get { return width; } }

        /// <summary>
        /// Loads the level editor, properly sizes it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LevelEditor_Load(object sender, EventArgs e)
        {           
            ResizeForm();
            texture1.Load("../../../Default size/1.png");
            texture1.SizeMode = PictureBoxSizeMode.Zoom;
            texture2.Load("../../../Default size/2.png");
            texture2.SizeMode = PictureBoxSizeMode.Zoom;
            texture3.Load("../../../Default size/3.png");
            texture3.SizeMode = PictureBoxSizeMode.Zoom;
            texture4.Load("../../../Default size/4.png");
            texture4.SizeMode = PictureBoxSizeMode.Zoom;
            texture5.Load("../../../Default size/5.png");
            texture5.SizeMode = PictureBoxSizeMode.Zoom;
            texture6.Load("../../../Default size/6.png");
            texture6.SizeMode = PictureBoxSizeMode.Zoom;
            texture7.Load("../../../Default size/7.png");
            texture7.SizeMode = PictureBoxSizeMode.Zoom;
            texture8.Load("../../../Default size/8.png");
            texture8.SizeMode = PictureBoxSizeMode.Zoom;

        }

        /// <summary>
        /// Tracks clicking the change color buttons, 
        /// or the picture boxes in the level
        /// </summary>
        /// <param name="sender">a click</param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {

            //PictureBox is detected
            if(sender is PictureBox)
            {
                //No path selected, do not change
                //the picture displayed on a picture box
                if(path == null)
                {
                    return;
                }

                PictureBox p = (PictureBox)sender;

                int widthLoc = p.Location.X / p.Width;
                int heightLoc = p.Location.Y / p.Height;

                p.Capture = false;

                //Allows for drawing pictures
                if(Control.MouseButtons == MouseButtons.Left)
                {
                    //Disposes image that existed before
                    //clicking on the box
                    if (p.Image != null)
                    {
                        p.Image.Dispose();
                    }

                    metadataStorage[heightLoc, widthLoc] = metadata;
                    //Resize image
                    p.SizeMode = PictureBoxSizeMode.Zoom;
                    //Load image
                    p.Load(path);
                    if (metadata == "" && boxes[0, 0].Enabled == true)
                        p.Load(path.Replace(".png", "Dim.png"));
                    p.BackColor = Color.Transparent;
                    Rotate(p);

                    if(boxes[0,0].Enabled == true)
                    {
                        rotationValues[heightLoc, widthLoc] = (int)rotation;
                    }
                    else if (collisions[0, 0].Enabled == true)
                    {
                        collisionRotations[heightLoc, widthLoc] = (int)rotation;
                    }

                    //Shows indicator that the user needs to save!
                    if (this.Text.IndexOf("*") == -1)
                    {
                        //Puts an asterisk if there are unsaved changes
                        this.Text = this.Text + "*";

                        //Unsaved changes...
                        isSaved = false;
                    }                 
                }

                //Draws colors
                else if(Control.MouseButtons == MouseButtons.Right)
                {
                    p.Image = null;

                    p.BackColor = currentColor;

                    metadataStorage[heightLoc, widthLoc] = "";
                    if (this.Text.IndexOf("*") == -1)
                    {
                        //Puts an asterisk if there are unsaved changes
                        this.Text = this.Text + "*";

                        //Unsaved changes...
                        isSaved = false;
                    }
                }
            }                                 
        }

        /// <summary>
        /// Activates when the user clicks the "save"
        /// button, saves the file to a .level extension
        /// </summary>
        /// <param name="sender">the button click</param>
        /// <param name="e">tracks events</param>
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveMenu = new SaveFileDialog();

            saveMenu.Filter = "Level Files|*.level";
            saveMenu.Title = "Save a level";
            DialogResult r = saveMenu.ShowDialog();

            if(r == DialogResult.OK)
            {

                stream = null;
                BinaryWriter writer = null;
                
                //File order
                //Width, height, LEVEL texture data,
                //Special Vector2/begin data.

                //Repeat again for overlay data.

                try
                {
                    if (firstFileName == null)
                    {
                        firstFileName = saveMenu.FileName.Remove(0, saveMenu.FileName.LastIndexOf('\\') + 1);
                        firstFileName = firstFileName.Substring(0, firstFileName.LastIndexOf('.'));
                        firstFilePath = saveMenu.FileName.Substring(0, saveMenu.FileName.LastIndexOf('\\'));
                        this.Text = $"Level Editor - {firstFileName + $"_{numSections}" + ".level"}";
                    }

                    stream = new FileStream($"{firstFilePath}\\" + firstFileName + $"_{numSections}.level", FileMode.Create);

                    writer = new BinaryWriter(stream);

                    //Saves the width and height
                    writer.Write(width);
                    writer.Write(height);

                    //Background

                    //Saves the ARGB colors of the pictureboxes
                    rotationSaveX = 0;
                    rotationSaveY = 0;
                    Save(writer, rotationValues);
                    writer.Close();
                    
                    //prompts the user that the file was successfully saved.
                    MessageBox.Show("Successfully Saved the file!", ":)");

                    

                    isSaved = true;

                    GenerateBoxes("../../../default-min.png");

                }
                catch(Exception ex)
                {
                    //Something went wrong...
                    MessageBox.Show("Error saving file! " + ex.Message, ":(");
                }
                finally
                {
                    //close the stream
                    if(stream != null)
                    {
                        writer.Close();
                    }                    
                }
            }
        }

        /// <summary>
        /// Activates when the user clicks the 
        /// load button.
        /// Purpose: Loads a .level file
        /// </summary>
        /// <param name="sender">the button click</param>
        /// <param name="e">tracks events</param>
        public void loadButton_Click(object sender, EventArgs e)
        {
            

            if (!isSaved)
            {
                DialogResult result =
                    MessageBox.Show("Would you like to save this file?",
                    "Save?",
                    MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    saveButton_Click(sender, e);
                }
                else if(result == DialogResult.Abort)
                {
                    return;
                }
            }

            //File order - 
            //Width, height, ARGB colors (all int32)
            OpenFileDialog dialog = new OpenFileDialog();

            BinaryReader reader = null;
            FileStream stream = null;

            dialog.Filter = "Level Files|*.level";
            dialog.Title = "Load a level";

            DialogResult r = dialog.ShowDialog();


            try
            {
                if (r == DialogResult.OK)
                {
                    //Loads the data from an external file
                    stream = new FileStream(dialog.FileName, FileMode.Open);

                    reader = new BinaryReader(stream);

                    //Reads width, height
                    width = reader.ReadInt32();
                    height = reader.ReadInt32();

                    //creates a new array that is able to store colors
                    colors = new string[height, width];
                    collisionColors = new string[height, width];
                    objectColors = new string[height, width];

                    firstFileName = dialog.FileName.Remove(0, dialog.FileName.LastIndexOf('\\') + 1);
                    firstFileName = firstFileName.Substring(0, firstFileName.LastIndexOf('.'));
                    string number = firstFileName[firstFileName.Length - 1].ToString();
                    numSections = int.Parse(number);
                    firstFileName = firstFileName.Substring(0, firstFileName.LastIndexOf('_'));
                    
                    firstFilePath = dialog.FileName.Substring(0, dialog.FileName.LastIndexOf('\\'));

                    //Background

                    rotationValues = new int[height, width];
                    collisionRotations = new int[height, width];

                    LoadColors(reader, rotationValues, false);

                    LoadBoxes(colors, collisionColors, objectColors);
                }

                //User cancels decision
                else if (r == DialogResult.Cancel)
                {
                    return;
                }

                //Displays the data on the map

                //Resizes the form accordingly
                ResizeForm();

                //formats the file name
                this.Text = $"Level editor - {dialog.FileName.Remove(0, dialog.FileName.LastIndexOf('\\') + 1)}";

                

                //prompts the user that the operation was successful
                MessageBox.Show("Successfully loaded the file!", ":)");

                backgroundButton.BackColor = Color.Green;
                collisionsButton.BackColor = Color.LavenderBlush;
            }

            catch (Exception ex)
            {
                //Error occurred...
                MessageBox.Show("Error Reading File! " + ex.Message, ":(");
            }

            finally
            {
                //Closes the stream
                if (stream != null)
                {
                    reader.Close();
                    rotation = 0;
                    Rotate(texturePic);
                    texturePic.Refresh();
                }
            }
        }

        /// <summary>
        /// Generates a new set of PictureBoxes, no load
        /// data required
        /// </summary>
        /// <param name="color">The color of the boxes</param>
        public void GenerateBoxes(string path)
        {

            if (biggerWidth && biggerHeight)
            {
                originalBoxHeight = 0;
                originalBoxWidth = 0;
                biggerWidth = false;
                biggerHeight = false;
            }

            if (width > height || width == height) biggerWidth = true;
            else if (height > width) biggerHeight = true;

            
            mapBox.Controls.Clear();
            //Handles Y axis
            for (int i = 0; i < height; i++)
            {
               
                //HAndles X axis
                for (int j = 0; j < width; j++)
                {
                    //Creates width # of Picture boxes height # of times
                    PictureBox box = new PictureBox();
                    PictureBox collisionBox = new PictureBox();
                    PictureBox objectBox = new PictureBox();
                   
                    //Sizes the boxes to the map width/height
                    //Horizontal is bigger, base off of the width

                    if(originalBoxHeight == 0 && originalBoxWidth == 0)
                    {
                        if (width > height || width == height)
                        {
                            box.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                            collisionBox.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                            objectBox.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                        }

                        //Vertical is bigger, base off of the height
                        else if (height > width)
                        {
                            box.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);
                            collisionBox.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);
                            objectBox.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);
                        }

                        originalBoxWidth = box.Width;
                        originalBoxHeight = box.Height;
                    }
                    else
                    {
                        box.Size = new Size(originalBoxWidth, originalBoxHeight);
                        collisionBox.Size = new Size(originalBoxWidth, originalBoxHeight);
                        objectBox.Size = new Size(originalBoxWidth, originalBoxHeight);
                    }
                    
                    //Sets the location of the box
                    //by taking the iteration of i and j.
                    box.Location = new Point
                        (10 + box.Width * j, 15 + box.Height * i);

                    collisionBox.Location = new Point
                        (10 + box.Width * j, 15 + box.Height * i);
                    
                    objectBox.Location = new Point
                        (10 + box.Width * j, 15 + box.Height * i);

                    //Boxes are visible
                    box.Visible = true;
                    collisionBox.Visible = true;
                    objectBox.Visible = true;
                    //Adds the box to the list of controls (for the group box)
                    mapBox.Controls.Add(box);
                    //Subscribes box's MouseDown to the button_Click method, so
                    //it is interactable.

                    //Allows for drag clicking
                    box.MouseDown += button_Click;
                    box.MouseEnter += button_Click;
                    
                    collisionBox.MouseDown += button_Click;
                    collisionBox.MouseEnter += button_Click;

                    objectBox.MouseDown += button_Click;
                    objectBox.MouseEnter += button_Click;


                    box.BackColor = Color.White;
                    collisionBox.BackColor = Color.White;

                    objectBox.BackColor = Color.White;
                    //saves in an array in case the data
                    //is saved to an external file
                    boxes[i, j] = box;
                    collisions[i, j] = collisionBox;
                    objects[i, j] = objectBox;
                    metadataStorage[i, j] = "";
                }
            }

            //resizes the form accordingly
            ResizeForm();
        }

        /// <summary>
        /// Like GenerateBoxes, but takes in loaded color 
        /// data and loads the boxes based on that
        /// </summary>
        /// <param name="colors">The array of loaded colors</param>
        public void LoadBoxes(string[,] colors, string[,] collisionColors, string[,] objectColors)
        {
            //Clears the controls (so we dont 
            //get overlap)

            mapBox.Controls.Clear();

           
            //Most of the functionality here is the same
            //as the above method, look there for non-unique 
            //comments
            boxes = new PictureBox[height, width];

            LoadPictureBoxLists(boxes, Width, Height, colors, rotationValues, true);
            LoadPictureBoxLists(collisions, Width, Height, collisionColors, collisionRotations);
            LoadPictureBoxLists(objects, Width, Height, objectColors, objectRotations);
        }

        /// <summary>
        /// Resizes the form and group box
        /// around the generated boxes
        /// </summary>
        public void ResizeForm()
        {
            int width = 0;
            int height = 0;

            //gets the width and height of 
            //an individual box
            boxWidth = boxes[0, 0].Width;
            boxHeight = boxes[0, 0].Height;

            //calculates the total width that the boxes
            //take up
            for(int i = 0; i < Width; i++)
            {
                width += boxWidth;
            }

            //calculates the total height that the boxes
            //take up
            for(int i = 0; i < Height; i++)
            {
                height += boxHeight;
            }

            //resizes the group box, with 25 pixels in each direction
            //for padding
            mapBox.Size = new Size(width + 25, height + 25);

            //resizes the form according to the width of the
            //group box
            this.Size = new Size(350 + mapBox.Width, 750);
        }

        /// <summary>
        /// Determines whether or not 
        /// a prompt needs to be shown
        /// if a user tries to close the program
        /// </summary>
        /// <param name="sender">'x' button click</param>
        /// <param name="e"></param>
        private void LevelEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Level has unsaved changes
            if(isSaved == false)
            {
                //prompts the user that they have unsaved changes
                DialogResult r = MessageBox.Show("There" +
                    " are unsaved changes. Are you sure you want to quit?", 
                    "Unsaved changes", 
                    MessageBoxButtons.YesNo);

                //Force quit
                if(r == DialogResult.Yes)
                {
                    e.Cancel = false;
                }
               
                //Doesn't exit the progarm
                else if(r == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        /// <summary>
        /// Displays recently selected tiles
        /// in a ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecentSelect(object sender, EventArgs e)
        {
            //Works much the same as pictureSelect_SelectedIndexChanged(object sender, EventArgs e)
            //but is operated in a different section.
            if (sender is PictureBox)
            {
                PictureBox p = (PictureBox)sender;

                path = p.ImageLocation;

                texturePic.Load(path);
                Rotate(texturePic);
                texturePic.Refresh();
            }
        }

        /// <summary>
        /// enables the object layer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void objButton_Click(object sender, EventArgs e)
        {
            active = 3;
            for (int i = 0; i < objects.GetLength(0); i++)
            {
                for (int j = 0; j < objects.GetLength(1); j++)
                {
                    if (objects[i, j].Image != null)
                    {
                        continue;
                    }
                    else
                    {
                        if (boxes[i, j].Image != null)
                        {
                            objects[i, j].BackColor = Color.Pink;
                        }
                    }
                }
            }
            rotateTexture.Enabled = false;


            mapBox.Controls.Clear();
            rotation = 0;
            texturePic.Image.Dispose();
            texturePic.Load(path);
            texturePic.Refresh();
            paintButton.Enabled = false;

            textures.Text = "Objects";

            texture1.Load("../../../Default size/1000.png");
            texture1.SizeMode = PictureBoxSizeMode.Zoom;
            texture2.Load("../../../Default size/1001.png");
            texture2.SizeMode = PictureBoxSizeMode.Zoom;
            texture3.Load("../../../Default size/1002.png");
            texture3.SizeMode = PictureBoxSizeMode.Zoom;
            texture4.Load("../../../Default size/1003.png");
            texture4.SizeMode = PictureBoxSizeMode.Zoom;
            texture5.Load("../../../Default size/1004.png");
            texture5.SizeMode = PictureBoxSizeMode.Zoom;
            texture6.Load("../../../Default size/1005.png");
            texture6.SizeMode = PictureBoxSizeMode.Zoom;

            path = "../../../Default size/1000.png";
            texturePic.Load("../../../Default size/1000.png");
            texturePic.Refresh();
            Rotate(texturePic);
            
            texture7.Image = null;
            texture7.Enabled = false;
            texture8.Image = null;
            texture8.Enabled = false;

            //changes color of buttons to indicate that the user
            //is selecting the background later
            collisionsButton.BackColor = Color.LavenderBlush;
            backgroundButton.BackColor = Color.LavenderBlush;
            objButton.BackColor = Color.Green;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    boxes[i, j].Enabled = false;
                    collisions[i, j].Enabled = false;
                    objects[i, j].Enabled = true;

                    mapBox.Controls.Add(objects[i, j]);
                }
            }
        }

        /// <summary>
        /// Changes to the background board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundButton_Click(object sender, EventArgs e)
        {
            active = 1;
            rotateTexture.Enabled = true;

            textures.Text = "Textures";

            texture1.Load("../../../Default size/1.png");
            texture1.SizeMode = PictureBoxSizeMode.Zoom;
            texture2.Load("../../../Default size/2.png");
            texture2.SizeMode = PictureBoxSizeMode.Zoom;
            texture3.Load("../../../Default size/3.png");
            texture3.SizeMode = PictureBoxSizeMode.Zoom;
            texture4.Load("../../../Default size/4.png");
            texture4.SizeMode = PictureBoxSizeMode.Zoom;
            texture5.Load("../../../Default size/5.png");
            texture5.SizeMode = PictureBoxSizeMode.Zoom;
            texture6.Load("../../../Default size/6.png");
            texture6.SizeMode = PictureBoxSizeMode.Zoom;
            texture7.Load("../../../Default size/7.png");
            texture7.SizeMode = PictureBoxSizeMode.Zoom;
            texture8.Load("../../../Default size/8.png");

            path = "../../../Default size/1.png";
            texturePic.Load("../../../Default size/1.png");
            texturePic.Refresh();
            Rotate(texturePic);

            texture4.Enabled = true;
            texture5.Enabled = true;
            texture6.Enabled = true;
            texture7.Enabled = true;
            texture8.Enabled = true;

            texture8.SizeMode = PictureBoxSizeMode.Zoom;
            //clears the controls
            mapBox.Controls.Clear();
            rotation = 0;
            texturePic.Image.Dispose();
            texturePic.Load(path);
            texturePic.Refresh();
            paintButton.Enabled = true;

            //changes color of buttons to indicate that the user
            //is selecting the background later
            collisionsButton.BackColor = Color.LavenderBlush;
            backgroundButton.BackColor = Color.Green;
            objButton.BackColor = Color.LavenderBlush;

            //replaces pictureBox data with the background data
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    boxes[i, j].Enabled = true;
                    collisions[i, j].Enabled = false;

                    mapBox.Controls.Add(boxes[i, j]);
                }
            }
        }

        /// <summary>
        /// Switches the current active layer to the
        /// collisions layer
        /// </summary>
        /// <param name="sender">collisions layer button</param>
        /// <param name="e">Handles events</param>
        private void collisionsButton_Click(object sender, EventArgs e)
        {
            active = 2;
            rotateTexture.Enabled = false;

            for (int i = 0; i < collisions.GetLength(0); i++)
            {
                for (int j = 0; j < collisions.GetLength(1); j++)
                {
                    if (collisions[i, j].Image != null)
                    {
                        continue;
                    }
                    else
                    {
                        if (boxes[i, j].Image != null)
                        {
                            collisions[i, j].BackColor = Color.Red;
                        }
                    }
                }
            }


            mapBox.Controls.Clear();
            rotation = 0;
            texturePic.Image.Dispose();
            texturePic.Load(path);
            texturePic.Refresh();
            paintButton.Enabled = true;

            textures.Text = "Collision Types";

            texture1.Load("../../../Default size/100.png");
            texture1.SizeMode = PictureBoxSizeMode.Zoom;
            texture2.Load("../../../Default size/101.png");
            texture2.SizeMode = PictureBoxSizeMode.Zoom;
            texture3.Load("../../../Default size/102.png");
            texture3.SizeMode = PictureBoxSizeMode.Zoom;
            texture4.Load("../../../Default size/103.png");
            texture4.SizeMode = PictureBoxSizeMode.Zoom;
            texture5.Load("../../../Default size/104.png");
            texture5.SizeMode = PictureBoxSizeMode.Zoom;
            texture6.Load("../../../Default size/105.png");
            texture6.SizeMode = PictureBoxSizeMode.Zoom;

            path = "../../../Default size/100.png";
            texturePic.Load("../../../Default size/100.png");
            texturePic.Refresh();
            Rotate(texturePic);

            texture4.Enabled = true;
            texture5.Enabled = true;
            texture6.Enabled = true;

            texture7.Image = null;
            texture7.Enabled = false;
            texture8.Image = null;
            texture8.Enabled = false;

            //changes color of buttons to indicate that the user
            //is selecting the background later
            collisionsButton.BackColor = Color.Green;
            backgroundButton.BackColor = Color.LavenderBlush;
            objButton.BackColor = Color.LavenderBlush;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    boxes[i, j].Enabled = false;
                    collisions[i, j].Enabled = true;

                    mapBox.Controls.Add(collisions[i, j]);
                }
            }
        }

        /// <summary>
        /// A method that simplifies loading the data
        /// of picture boxes
        /// </summary>
        /// <param name="boxes">list of picture boxes</param>
        /// <param name="width">width of the box</param>
        /// <param name="height">height of the box</param>
        /// <param name="codeList">list of image paths</param>
        private void LoadPictureBoxLists(PictureBox[,] boxes,
            int width, int height, string[,] codeList, int[,] rotationValues, bool Dim = false)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //creates a new picture box
                    PictureBox box = new PictureBox();

                    if (originalBoxHeight == 0 && originalBoxWidth == 0)
                    {
                        if (width > height || width == height)
                        {
                            box.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                        }

                        //Vertical is bigger, base off of the height
                        else if (height > width)
                        {
                            box.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);
                        }

                        originalBoxWidth = box.Width;
                        originalBoxHeight = box.Height;
                    }
                    else
                    {
                        box.Size = new Size(originalBoxWidth, originalBoxHeight);
                    }

                    //Puts a location to the box
                    box.Location = new Point
                        (10 + box.Width * j, 15 + box.Height * i);

                    box.Visible = true;

                    //subscribes the box to various methods
                    box.MouseDown += button_Click;
                    box.MouseEnter += button_Click;

                    //assigns a box the color from the corresponding
                    //colors array index


                    //Default picture detected, Color the tile white.
                    if (codeList[i, j] == "../../../Default size/9.png")
                    {
                        box.BackColor = Color.White;
                    }
                    else
                    {
                        //loads the img
                        box.Load(codeList[i, j]);
                        if (metadataStorage[i, j] == "" && Dim)
                            box.Load(codeList[i, j].Replace(".png", "Dim.png"));
                        //resizes the image
                        box.SizeMode = PictureBoxSizeMode.Zoom;

                        rotation = rotationValues[i, j];
                        Rotate(box);
                        box.Refresh();                     
                    }

                    //Add the pictureboxes to the controls
                    mapBox.Controls.Add(box);

                    //add data to 2d arrays
                    boxes[i, j] = box;
                }
            }
        }

        /// <summary>
        /// Simplifies the code when loading in files, allows
        /// the user to specify which list they want data stored in
        /// instead of hard coding it
        /// </summary>
        /// <param name="codeList">array of data</param>
        /// <param name="reader">reads in data</param>
        public void LoadColors(BinaryReader reader,
            int[,] rotationValues, bool old)
        {
            byte version = (byte)reader.PeekChar();
            //Fills the array with the colors of the loaded data
            //(Vector2/Begin tile/track)
            for (int i = 0; i < colors.GetLength(0); i++)
            {
                for (int j = 0; j < colors.GetLength(1); j++)
                {
                    string currentPicture = "../../../Default size/" + reader.ReadString() + ".png";
                    colors[i, j] = currentPicture;

                    int rotationValue = reader.ReadInt32();
                    rotationValues[i, j] = rotationValue;

                    string currentCollision = "../../../Default size/" + reader.ReadString() + ".png";
                    collisionColors[i, j] = currentCollision;

                    string currentObject = "../../../Default size/" + reader.ReadString() + ".png";
                    objectColors[i, j] = currentObject;
                    if (!old)
                    {
                        string currentMetadata = reader.ReadString().TrimStart('m');
                        metadataStorage[i, j] = currentMetadata;
                    } else {
                        metadataStorage[i, j] = "";
                    }
                }
            }
        }

        /// <summary>
        /// Changes the image displayed
        /// in the currently selected tile 
        /// picture box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangePath(Object sender, EventArgs e)
        {
            if(sender is PictureBox)
            {
                PictureBox p = (PictureBox)sender;

                path = p.ImageLocation;

                //removes the image
                texturePic.Image.Dispose();
                //loads a new image
                texturePic.Load(path);
                //rotates the image (if necessary)
                Rotate(texturePic);
                //refreshes the box to show the change
                texturePic.Refresh();
            }
        }

        /// <summary
        /// Facilitates saving files
        /// </summary>
        /// <param name="boxes">current List of pictureBoxes</param>
        /// <param name="writer">BinaryWriter</param>
        private void Save(BinaryWriter writer,
            int[,] rotationValues)
        {
            rotationSaveX = 0;
            rotationSaveY = 0;

            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for(int j = 0; j < boxes.GetLength(1); j++)
                {
                    //writes the location of the image
                    if (boxes[i, j].Image != null)
                    {
                        writer.Write(
                            boxes[i, j].ImageLocation.Substring(
                                boxes[i, j].ImageLocation.LastIndexOf('/') + 1,
                                boxes[i, j].ImageLocation.LastIndexOf('.') - 
                                boxes[i, j].ImageLocation.LastIndexOf('/') - 1)
                            .Replace("Dim", ""));
                    }
                    else
                    {
                        writer.Write("9");
                    }

                    writer.Write(rotationValues[rotationSaveY, rotationSaveX]);
                    rotationSaveX++;
                    if (rotationSaveX == Width)
                    {
                        rotationSaveY++;
                        rotationSaveX = 0;
                    }

                    //writes the location of the image
                    if (collisions[i, j].Image != null)
                    {
                        writer.Write(
                            collisions[i, j].ImageLocation.Substring(
                                collisions[i, j].ImageLocation.LastIndexOf('/') + 1,
                                collisions[i, j].ImageLocation.LastIndexOf('.') - 
                                collisions[i, j].ImageLocation.LastIndexOf('/') - 1));
                    }
                    else
                    {
                        writer.Write("9");
                    }

                    //writes the location of the image
                    if (objects[i, j].Image != null)
                    {
                        writer.Write(
                            objects[i, j].ImageLocation.Substring(
                                objects[i, j].ImageLocation.LastIndexOf('/') + 1,
                                objects[i, j].ImageLocation.LastIndexOf('.') -
                                objects[i, j].ImageLocation.LastIndexOf('/') - 1));
                    }
                    else
                    {
                        writer.Write("9");
                    }

                    //writes the location of the image
                    writer.Write("m" + metadataStorage[i, j]);
                }
            }
        }

        /// <summary>
        /// Rotates a texture on the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rotateTexture_Click(object sender, EventArgs e)
        {
            if(rotation == 360)
            {
                rotation = 0;
            }

            //rotates the texturePic image
            texturePic.Image.RotateFlip
                (RotateFlipType.Rotate90FlipNone);
            texturePic.Refresh();

            //adds to the current rotatiom
            rotation += 90f;
        }

        /// <summary>
        /// Rotates an image inside of a pictureBox
        /// </summary>
        /// <param name="p"></param>
        private void Rotate(PictureBox p)
        {
            if (rotation == 90)
            {
                //rotates the image 90 degrees
                p.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }
            else if (rotation == 180)
            {
                //rotates the image 180 degrees
                p.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            }
            else if (rotation == 270)
            {
                //rotates the image 270 degrees
                p.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
        }

        /// <summary>
        /// Paints all tiles to one tile
        /// (Background layer only)
        /// </summary>
        /// <param name="sender">button</param>
        /// <param name="e">handles events</param>
        public void PaintBox(object sender, EventArgs e)
        {
            if (active == 1)
            {
                if (sender is Button)
                {
                    //Makes sure the user wants to recolor the
                    //entire background by showing a popup
                    DialogResult rs =
                        MessageBox.Show(
                            "Are you sure you want to recolor the whole background?",
                            "?",
                            MessageBoxButtons.YesNo);

                    //user wants to recolor the whole background
                    if (rs == DialogResult.Yes)
                    {
                        //goes through all of the pictureBoxes
                        foreach (PictureBox p in boxes)
                        {
                            //checks if an image is present
                            if (p.Image != null)
                            {
                                p.Image.Dispose();
                            }
                            //Replaces the image
                            p.Load(path);
                            if (metadata == "" && boxes[0, 0].Enabled == true)
                                p.Load(path.Replace(".png", "Dim.png"));
                            p.SizeMode = PictureBoxSizeMode.Zoom;
                            Rotate(p);
                            p.Update();

                            int widthLoc = p.Location.X / p.Width;
                            int heightLoc = p.Location.Y / p.Height;

                            if (boxes[0, 0].Enabled == true)
                            {
                                rotationValues[heightLoc, widthLoc] = (int)rotation;
                            }
                            else if (collisions[0, 0].Enabled == true)
                            {
                                collisionRotations[heightLoc, widthLoc] = (int)rotation;
                            }
                        }
                    }
                }
            }
            else if (active == 2)
            {
                if (sender is Button)
                {
                    //Makes sure the user wants to recolor the
                    //entire background by showing a popup
                    DialogResult rs =
                        MessageBox.Show(
                            "Are you sure you want to fill in all collisions?",
                            "?",
                            MessageBoxButtons.YesNo);

                    //user wants to recolor the whole background
                    if (rs == DialogResult.Yes)
                    {
                        //goes through all of the pictureBoxes
                        foreach (PictureBox p in collisions)
                        {
                            
                                
                            //checks if an image is present
                            if (p.BackColor == Color.Red)
                            {
                                if (p.Image != null)
                                    p.Image.Dispose();

                                p.Load(path);

                                if (metadata == "" && boxes[0, 0].Enabled == true)
                                    p.Load(path.Replace(".png", "Dim.png"));
                                p.SizeMode = PictureBoxSizeMode.Zoom;
                                Rotate(p);
                                p.Update();                              
                            }
                            //Replaces the image
                            

                            int widthLoc = p.Location.X / p.Width;
                            int heightLoc = p.Location.Y / p.Height;

                            if (boxes[0, 0].Enabled == true)
                            {
                                rotationValues[heightLoc, widthLoc] = (int)rotation;
                            }
                            else if (collisions[0, 0].Enabled == true)
                            {
                                collisionRotations[heightLoc, widthLoc] = (int)rotation;
                            }
                        }
                    }
                }
            }         
        }

        private void Metadata_Click(object sender, EventArgs e)
        {
            InputBox.SetLanguage(InputBox.Language.English);

            DialogResult res = InputBox.ShowDialog("Insert the metadata of the tile below:", "Metadata");

            if (res == System.Windows.Forms.DialogResult.OK || res == System.Windows.Forms.DialogResult.Yes)
                metadata = InputBox.ResultValue;
            Metadata.Text = $"Metadata: \"{metadata}\"";
        }

        private void oldLoadButton_Click(object sender, EventArgs e)
        {
            //File order - 
            //Width, height, ARGB colors (all int32)
            OpenFileDialog dialog = new OpenFileDialog();

            BinaryReader reader = null;
            FileStream stream = null;

            dialog.Filter = "Level Files|*.level";
            dialog.Title = "Load a level";

            DialogResult r = dialog.ShowDialog();


            try
            {
                if (r == DialogResult.OK)
                {
                    //Loads the data from an external file
                    stream = new FileStream(dialog.FileName, FileMode.Open);

                    reader = new BinaryReader(stream);

                    //Reads width, height
                    width = reader.ReadInt32();
                    height = reader.ReadInt32();

                    //creates a new array that is able to store colors
                    colors = new string[height, width];
                    collisionColors = new string[height, width];
                    objectColors = new string[height, width];

                    //Background

                    rotationValues = new int[height, width];
                    collisionRotations = new int[height, width];

                    LoadColors(reader, rotationValues, true);

                    LoadBoxes(colors, collisionColors, objectColors);
                }

                //User cancels decision
                else if (r == DialogResult.Cancel)
                {
                    return;
                }

                //Displays the data on the map

                //Resizes the form accordingly
                ResizeForm();

                //formats the file name
                this.Text = $"Level editor - {dialog.FileName.Remove(0, dialog.FileName.LastIndexOf('\\') + 1)}";

                
                
                //prompts the user that the operation was successful
                MessageBox.Show("Successfully loaded the file!", ":)");

                backgroundButton.BackColor = Color.Green;
                collisionsButton.BackColor = Color.LavenderBlush;
            }

            catch (Exception ex)
            {
                //Error occurred...
                MessageBox.Show("Error Reading File! " + ex.Message, ":(");
            }

            finally
            {
                //Closes the stream
                if (stream != null)
                {
                    reader.Close();
                    rotation = 0;
                    Rotate(texturePic);
                    texturePic.Refresh();
                }
            }
        }

        /// <summary>
        /// Moves on to the next area of the level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextButton_Click(object sender, EventArgs e)
        {
            

            if (firstFileName == null)
            {
                DialogResult result =
                    MessageBox.Show("Would you like to save this file?",
                    "Save?",
                    MessageBoxButtons.YesNo);             

                if (result == DialogResult.No) return;
                else if (result == DialogResult.Yes)
                {
                    saveButton_Click(sender, e);
                    numSections++;
                    this.Text = $"Level Editor - {firstFileName + $"_{numSections}" + ".level"}";
                }
                else return;
            }
            
            if (!isSaved)
            {
                BinaryWriter writer = null;
                try
                {
                    stream = new FileStream($"{firstFilePath}\\"
                        + $"{firstFileName}" + $"_{numSections}" + ".level", FileMode.Create);

                    writer = new BinaryWriter(stream);

                    //Saves the width and height
                    writer.Write(width);
                    writer.Write(height);

                    //Background

                    //Saves the ARGB colors of the pictureboxes
                    rotationSaveX = 0;
                    rotationSaveY = 0;
                    Save(writer, rotationValues);
                    writer.Close();

                    isSaved = true;

                }
                catch (Exception ex)
                {
                    //Something went wrong...
                    MessageBox.Show("Error saving file! " + ex.Message, ":(");
                }
                finally
                {
                    stream.Close();
                }
            }
            
            if (!File.Exists($"{firstFilePath}\\"
                        + $"{firstFileName}" + $"_{numSections + 1}" + ".level") && numSections + 1 != 0)
            {
                BinaryWriter writer = null;
                try
                {
                    stream = new FileStream($"{firstFilePath}\\" 
                        + $"{firstFileName}" + $"_{numSections}" + ".level", FileMode.Create);

                    writer = new BinaryWriter(stream);

                    //Saves the width and height
                    writer.Write(width);
                    writer.Write(height);

                    //Background

                    //Saves the ARGB colors of the pictureboxes
                    rotationSaveX = 0;
                    rotationSaveY = 0;
                    Save(writer, rotationValues);
                    writer.Close();
                    
                    isSaved = true;

                }
                catch (Exception ex)
                {
                    //Something went wrong...
                    MessageBox.Show("Error saving file! " + ex.Message, ":(");
                }
                finally
                {
                    //close the stream
                    if (stream != null)
                    {
                        numSections++;
                        this.Text = $"Level Editor - {firstFileName + $"_{numSections}" + ".level"}";
                        writer.Close();
                    }
                }

                GenerateBoxes("../../../default-min.png");           
            }
            else
            {
                numSections++;
                LoadLevelInEditor();               
                this.Text = $"Level Editor - {firstFileName + $"_{numSections}" + ".level"}";
            }

            isSaved = false;
        }

        /// <summary>
        /// Moves onto the previous area of the level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevButton_Click(object sender, EventArgs e)
        {
            if(firstFileName == null || numSections - 1 < 0)
            {
                return;
            }
            else
            {
                BinaryWriter writer = null;
                try
                {
                    if (!isSaved)
                    {
                        stream = new FileStream($"{firstFilePath}\\"
                        + $"{firstFileName}" + $"_{numSections}" + ".level", FileMode.Create);

                        writer = new BinaryWriter(stream);

                        //Saves the width and height
                        writer.Write(width);
                        writer.Write(height);

                        //Background

                        //Saves the ARGB colors of the pictureboxes
                        rotationSaveX = 0;
                        rotationSaveY = 0;
                        Save(writer, rotationValues);
                        writer.Close();

                        isSaved = true;
                    }
                   
                }
                catch (Exception ex)
                {
                    //Something went wrong...
                    MessageBox.Show("Error saving file! " + ex.Message, ":(");
                }
                finally
                {
                    //close the stream
                    if (stream != null)
                    {
                        numSections--;
                        LoadLevelInEditor();
                        this.Text = $"Level Editor - {firstFileName + $"_{numSections}" + ".level"}";
                        prevButtonHit = true;
                    }
                }

            }            
        }

        private void LoadLevelInEditor()
        {
            BinaryReader reader = null;
            try
            {
                if(numSections < 0)
                {
                    return;
                }
                else
                {
                    //Loads the data from an external file
                    stream = new FileStream($"{firstFilePath}\\" + $"{firstFileName}" + $"_{numSections}" + ".level", FileMode.Open);
                }


                reader = new BinaryReader(stream);

                //Reads width, height
                width = reader.ReadInt32();
                height = reader.ReadInt32();

                //creates a new array that is able to store colors
                colors = new string[height, width];
                collisionColors = new string[height, width];
                objectColors = new string[height, width];

                //Background

                rotationValues = new int[height, width];
                collisionRotations = new int[height, width];

                LoadColors(reader, rotationValues, false);

                LoadBoxes(colors, collisionColors, objectColors);
            }
            catch (Exception ex)
            {
                //Error occurred...
                MessageBox.Show("Error Reading File! " + ex.Message, ":(");
            }

            finally
            {
                //Closes the stream
                if (stream != null && reader != null)
                {
                    reader.Close();
                    rotation = 0;
                    Rotate(texturePic);
                    texturePic.Refresh();

                }
            }
        }



    }   
}
