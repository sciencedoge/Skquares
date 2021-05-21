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
        private List<Button> buttons;
        private FileStream stream;
        private string[,] colors;
        private string[,] collisionColors;
        private bool isSaved;
        private int rotationSaveX;
        private int rotationSaveY;
        private bool rotationsLoaded;


        private Color currentColor;

        private string path;
        private int[,] rotationValues;
        private int[,] collisionRotations;

        //ROTATION
        private float rotation;

        private int numLayersSaved;

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
        /// Returns a list of rotation values
        /// or alters the list of rotation values
        /// </summary>
        public int[,] CollisionValues
        {
            get { return collisionRotations; }
            set { collisionRotations = value; }
        }

        /// <summary>
        /// Returns whether or not 
        /// the rotations are loaded, or
        /// changes the variable to delcare
        /// whether or not the rotations are loaded
        /// </summary>
        public bool RotationsLoaded
        {
            get { return rotationsLoaded; }
            set { rotationsLoaded = value; }
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
            rotationValues = new int[height, width];
            collisionRotations = new int[height, width];
            isSaved = true;
            rotationsLoaded = false;
            numLayersSaved = 0;

            buttons = new List<Button>();

            currentColor = Color.Red;

            path = "../../../Default size/1.png";
            texturePic.Load(path);
            texturePic.SizeMode = PictureBoxSizeMode.Zoom;

            backgroundButton.BackColor = Color.Green;
            rotation = 0;
            rotationSaveX = 0;
            rotationSaveY = 0;
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
                    if(p.Image != null)
                    {
                        p.Image.Dispose();
                    }
                    
                    //Resize image
                    p.SizeMode = PictureBoxSizeMode.Zoom;
                    //Load image
                    p.Load(path);
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

            saveMenu.Filter = "Level Files|.level";
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
                    stream = new FileStream(saveMenu.FileName, FileMode.Create);

                    writer = new BinaryWriter(stream);

                    //Saves the width and height
                    writer.Write(width);
                    writer.Write(height);

                    //Background

                    //Saves the ARGB colors of the pictureboxes
                    rotationSaveX = 0;
                    rotationSaveY = 0;
                    Save(boxes, writer, rotationValues);
                    Save(collisions, writer, collisionRotations);

                    //prompts the user that the file was successfully saved.
                    MessageBox.Show("Successfully Saved the file!", ":)");
                    this.Text = $"Level Editor - {saveMenu.FileName.Remove(0, saveMenu.FileName.LastIndexOf('\\') + 1)}";
                    isSaved = true;
                
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

                    //Background

                    rotationValues = new int[height, width];
                    collisionRotations = new int[height, width];

                    LoadColors(colors, reader, rotationValues, rotationsLoaded);

                    //Collisions
                    LoadColors(collisionColors, reader, collisionRotations, rotationsLoaded);

                    LoadBoxes(colors, collisionColors);
                }

                //User cancels decision
                else if(r == DialogResult.Cancel)
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

            catch(Exception ex)
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
            //Handles Y axis
            for (int i = 0; i < height; i++)
            {
               
                //HAndles X axis
                for (int j = 0; j < width; j++)
                {
                    //Creates width # of Picture boxes height # of times
                    PictureBox box = new PictureBox();
                    PictureBox collisionBox = new PictureBox();
                   
                    //Sizes the boxes to the map width/height
                    //Horizontal is bigger, base off of the width
                    if (width > height || width == height)
                    {
                        box.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                        collisionBox.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                    }
                   
                    //Vertical is bigger, base off of the height
                    else if (height > width)
                    {
                        box.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);
                        collisionBox.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);

                    }
                    
                    //Sets the location of the box
                    //by taking the iteration of i and j.
                    box.Location = new Point
                        (10 + box.Width * j, 15 + box.Height * i);

                    collisionBox.Location = new Point
                        (10 + box.Width * j, 15 + box.Height * i);

                    //Boxes are visible
                    box.Visible = true;
                    collisionBox.Visible = true;
                    //Adds the box to the list of controls (for the group box)
                    mapBox.Controls.Add(box);
                    //Subscribes box's MouseDown to the button_Click method, so
                    //it is interactable.

                    //Allows for drag clicking
                    box.MouseDown += button_Click;
                    box.MouseEnter += button_Click;

                    collisionBox.MouseDown += button_Click;
                    collisionBox.MouseEnter += button_Click;


                    box.BackColor = Color.White;
                    collisionBox.BackColor = Color.White;
                    //saves in an array in case the data
                    //is saved to an external file
                    boxes[i, j] = box;
                    collisions[i, j] = collisionBox;
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
        public void LoadBoxes(string[,] colors, string[,] collisionColors)
        {
            //Clears the controls (so we dont 
            //get overlap)


            //Resizes the groupBox
            mapBox.Size = new Size(575, 575);

            mapBox.Controls.Clear();

           
            //Most of the functionality here is the same
            //as the above method, look there for non-unique 
            //comments
            boxes = new PictureBox[height, width];

            LoadPictureBoxLists(boxes, Width, Height, colors, rotationValues);
            rotationsLoaded = false;
            LoadPictureBoxLists(collisions, Width, Height, collisionColors, collisionRotations);
            
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
        /// Allows the user to choose between a set 
        /// number of colors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPicker(object sender, EventArgs e)
        {
            if(sender is Button)
            {
                Button b = (Button)(sender);

                //changes the current color selected
                currentColor = b.BackColor;           
            }
        }

        /// <summary>
        /// Changes to the background board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundButton_Click(object sender, EventArgs e)
        {
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
            mapBox.Controls.Clear();
            rotation = 0;
            texturePic.Image.Dispose();
            texturePic.Load(path);
            texturePic.Refresh();
            paintButton.Enabled = false;

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

            texture6.Image = null;
            texture6.Enabled = false;
            texture7.Image = null;
            texture7.Enabled = false;
            texture8.Image = null;
            texture8.Enabled = false;

            //changes color of buttons to indicate that the user
            //is selecting the background later
            collisionsButton.BackColor = Color.Green;
            backgroundButton.BackColor = Color.LavenderBlush;

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
            int width, int height, string[,] codeList, int[,] rotationValues)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    //creates a new picture box
                    PictureBox box = new PictureBox();

                    //sizes the boxes accordingly
                    if (width > height || width == height)
                    {
                        box.Size = new Size((mapBox.Width) / width, (mapBox.Width) / width);
                    }

                    else if (height > width)
                    {
                        box.Size = new Size((mapBox.Height - 17) / height, (mapBox.Height - 17) / height);
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
        public void LoadColors(string[,] codeList, BinaryReader reader,
            int[,] rotationValues, bool rotationsLoaded)
        {
            //Fills the array with the colors of the loaded data
            //(Vector2/Begin tile/track)
            for (int i = 0; i < codeList.GetLength(0); i++)
            {
                for (int j = 0; j < codeList.GetLength(1); j++)
                {
                    string currentPicture = "../../../Default size/" + reader.ReadString() + ".png";
                    codeList[i, j] = currentPicture;

                    int rotationValue = reader.ReadInt32();
                    rotationValues[i, j] = rotationValue;
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
        private void Save(PictureBox[,] boxes, BinaryWriter writer,
            int[,] rotationValues)
        {
            rotationSaveX = 0;
            rotationSaveY = 0;

            foreach (PictureBox b in boxes)
            {
                //writes the location of the image
                if (b.Image != null)
                {
                    writer.Write(b.ImageLocation.Substring(b.ImageLocation.LastIndexOf('/') + 1, 1));
                }
                else
                {
                    writer.Write("9");
                }

                writer.Write(rotationValues[rotationSaveY, rotationSaveX]);
                rotationSaveX++;
                if(rotationSaveX == Width)
                {
                    rotationSaveY++;
                    rotationSaveX = 0;
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
            if(sender is Button)
            {
                //Makes sure the user wants to recolor the
                //entire background by showing a popup
                DialogResult rs = 
                    MessageBox.Show(
                        "Are you sure you want to recolor the whole background?",
                        "?",
                        MessageBoxButtons.YesNo);

                //user wants to recolor the whole background
                if(rs == DialogResult.Yes)
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
    }   
}
