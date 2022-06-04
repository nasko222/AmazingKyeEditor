using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AmazingKyeEditor
{
    public partial class Form1 : Form
    {
        private int objectID;
        private int data0;
        private int data1;
        private int data2;
        private int x1;
        private int x2;
        private int x3;
        private int x4;
        private int y1;
        private byte[] NOFobjectID = new byte[600];
        private byte[] NOFdata0 = new byte[600];
        private byte[] NOFdata1 = new byte[600];
        private byte[] NOFdata2 = new byte[600];
        private byte[] NOFx1 = new byte[600];
        private byte[] NOFx2 = new byte[600];
        private byte[] NOFx3 = new byte[600];
        private byte[] NOFx4 = new byte[600];
        private byte[] NOFy1 = new byte[600];
        private int wallX = 1;
        private int wallY = 1;
        private Image[] kye = new Image[1];
        private Image[] diamond = new Image[1];
        private Image[] destroyers = new Image[1];
        private Image[] squaremovebox = new Image[4];
        private Image[] circlemovebox = new Image[4];
        private Image[] boxes = new Image[2];
        private Image[] ghostboxes = new Image[2];
        private Image[] pushers = new Image[4];
        private Image[] magnets = new Image[2];
        private Image[] rotators = new Image[2];
        private Image[] squaregenerators = new Image[4];
        private Image[] circlegenerators = new Image[4];
        private Image[] timers = new Image[10];
        private PictureBox[] tileField = new PictureBox[600];
        private Dictionary<int, int> levelTileHashes = new Dictionary<int, int>();
        private string LVLname;
        private string LVLintro;
        private string LVLhint;
        private string LVLwin;
        private string LVLid;
        private bool tutorialLevel;
        private int levelID;
        private int stars = 1;

        private static Bitmap spritesheet;

        private Image LoadSpriteFromSheet(int x, int y)
        {
            x--;
            y--;
            return spritesheet.Clone(new Rectangle(x * 20, y * 20, 20, 20), spritesheet.PixelFormat);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private string Between(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Load spritesheet into memory
            spritesheet = new Bitmap("skin_0.bmp");

            //Read sprites from spritesheet
            kye[0] = LoadSpriteFromSheet(1, 4);
            diamond[0] = LoadSpriteFromSheet(2, 4);
            destroyers[0] = LoadSpriteFromSheet(4, 3);
            squaremovebox[0] = LoadSpriteFromSheet(6, 1);
            squaremovebox[1] = LoadSpriteFromSheet(6, 2);
            squaremovebox[2] = LoadSpriteFromSheet(6, 3);
            squaremovebox[3] = LoadSpriteFromSheet(6, 4);
            circlemovebox[0] = LoadSpriteFromSheet(7, 1);
            circlemovebox[1] = LoadSpriteFromSheet(7, 2);
            circlemovebox[2] = LoadSpriteFromSheet(7, 3);
            circlemovebox[3] = LoadSpriteFromSheet(7, 4);
            boxes[0] = LoadSpriteFromSheet(8, 1);
            boxes[1] = LoadSpriteFromSheet(8, 2);
            ghostboxes[0] = LoadSpriteFromSheet(8, 3);
            ghostboxes[1] = LoadSpriteFromSheet(8, 4);
            pushers[0] = LoadSpriteFromSheet(9, 1);
            pushers[1] = LoadSpriteFromSheet(9, 2);
            pushers[2] = LoadSpriteFromSheet(9, 3);
            pushers[3] = LoadSpriteFromSheet(9, 4);
            magnets[0] = LoadSpriteFromSheet(10, 1);
            magnets[1] = LoadSpriteFromSheet(10, 2);
            rotators[0] = LoadSpriteFromSheet(10, 3);
            rotators[1] = LoadSpriteFromSheet(10, 4);
            squaregenerators[0] = LoadSpriteFromSheet(11, 1);
            squaregenerators[1] = LoadSpriteFromSheet(11, 2);
            squaregenerators[2] = LoadSpriteFromSheet(11, 3);
            squaregenerators[3] = LoadSpriteFromSheet(11, 4);
            circlegenerators[0] = LoadSpriteFromSheet(5, 1);
            circlegenerators[1] = LoadSpriteFromSheet(5, 2);
            circlegenerators[2] = LoadSpriteFromSheet(5, 3);
            circlegenerators[3] = LoadSpriteFromSheet(5, 4);
            timers[0] = LoadSpriteFromSheet(12, 1);
            timers[1] = LoadSpriteFromSheet(12, 2);
            timers[2] = LoadSpriteFromSheet(12, 3);
            timers[3] = LoadSpriteFromSheet(12, 4);
            timers[4] = LoadSpriteFromSheet(12, 5);
            timers[5] = LoadSpriteFromSheet(12, 6);
            timers[6] = LoadSpriteFromSheet(12, 7);
            timers[7] = LoadSpriteFromSheet(13, 1);
            timers[8] = LoadSpriteFromSheet(13, 2);
            timers[9] = LoadSpriteFromSheet(13, 3);

            //Texture editor buttons
            kye1.Image = kye[0];
            diamond1.Image = diamond[0];
            destroyer.Image = destroyers[0];
            squarep1.Image = squaremovebox[0];
            squarep2.Image = squaremovebox[1];
            squarep3.Image = squaremovebox[2];
            squarep4.Image = squaremovebox[3];
            circlep1.Image = circlemovebox[0];
            circlep2.Image = circlemovebox[1];
            circlep3.Image = circlemovebox[2];
            circlep4.Image = circlemovebox[3];
            box1.Image = boxes[0];
            box2.Image = boxes[1];
            ghostbox1.Image = ghostboxes[0];
            ghostbox2.Image = ghostboxes[1];
            pusher1.Image = pushers[0];
            pusher2.Image = pushers[1];
            pusher3.Image = pushers[2];
            pusher4.Image = pushers[3];
            magnetns.Image = magnets[0];
            magnetew.Image = magnets[1];
            rotateclock.Image = rotators[0];
            rotatecount.Image = rotators[1];
            squaregen1.Image = squaregenerators[0];
            squaregen2.Image = squaregenerators[1];
            squaregen3.Image = squaregenerators[2];
            squaregen4.Image = squaregenerators[3];
            circlegen1.Image = circlegenerators[0];
            circlegen2.Image = circlegenerators[1];
            circlegen3.Image = circlegenerators[2];
            circlegen4.Image = circlegenerators[3];
            timer0.Image = timers[0];
            timer1.Image = timers[1];
            timer2.Image = timers[2];
            timer3.Image = timers[3];
            timer4.Image = timers[4];
            timer5.Image = timers[5];
            timer6.Image = timers[6];
            timer7.Image = timers[7];
            timer8.Image = timers[8];
            timer9.Image = timers[9];

            //Render the rest
            pictureBox1.Image = spritesheet;
            levelBorder1.Image = LoadSpriteFromSheet(2, 2);
            RenderWallTile();

            //Generate the level tile field
            int tiles = 0;
            int ax = 38;
            int ay = 43;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    createLevelTile(ax, ay, tiles); tiles++; ay += 20;
                }
                ax += 20;
                ay = 43;
            }

            //Select Kye by default
            aSelected.Image = kye[0];
            objectID = 1;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;

            //Add safety wall

            //Corners
            tileField[0].Image = LoadSpriteFromSheet(7, 6);
            NOFobjectID[0] = 2;
            NOFdata0[0] = 0;
            NOFdata1[0] = 0;
            NOFdata2[0] = 0;
            NOFx1[0] = 7;
            NOFx2[0] = 0;
            NOFx3[0] = 0;
            NOFx4[0] = 0;
            NOFy1[0] = 6;

            tileField[19].Image = LoadSpriteFromSheet(6, 6);
            NOFobjectID[19] = 2;
            NOFdata0[19] = 0;
            NOFdata1[19] = 0;
            NOFdata2[19] = 0;
            NOFx1[19] = 6;
            NOFx2[19] = 0;
            NOFx3[19] = 0;
            NOFx4[19] = 0;
            NOFy1[19] = 6;

            tileField[580].Image = LoadSpriteFromSheet(5, 6);
            NOFobjectID[580] = 2;
            NOFdata0[580] = 0;
            NOFdata1[580] = 0;
            NOFdata2[580] = 0;
            NOFx1[580] = 5;
            NOFx2[580] = 0;
            NOFx3[580] = 0;
            NOFx4[580] = 0;
            NOFy1[580] = 6;

            tileField[599].Image = LoadSpriteFromSheet(8, 6);
            NOFobjectID[599] = 2;
            NOFdata0[599] = 0;
            NOFdata1[599] = 0;
            NOFdata2[599] = 0;
            NOFx1[599] = 8;
            NOFx2[599] = 0;
            NOFx3[599] = 0;
            NOFx4[599] = 0;
            NOFy1[599] = 6;



            //Left wall
            for (int k = 1; k < 19; k++)
            {
                tileField[k].Image = LoadSpriteFromSheet(3, 2);
                NOFobjectID[k] = 2;
                NOFdata0[k] = 0;
                NOFdata1[k] = 0;
                NOFdata2[k] = 0;
                NOFx1[k] = 3;
                NOFx2[k] = 0;
                NOFx3[k] = 0;
                NOFx4[k] = 0;
                NOFy1[k] = 2;
            }



            //Right wall
            for (int k = 581; k < 599; k++)
            {
                tileField[k].Image = LoadSpriteFromSheet(1, 2);
                NOFobjectID[k] = 2;
                NOFdata0[k] = 0;
                NOFdata1[k] = 0;
                NOFdata2[k] = 0;
                NOFx1[k] = 1;
                NOFx2[k] = 0;
                NOFx3[k] = 0;
                NOFx4[k] = 0;
                NOFy1[k] = 2;
            }



            //Top Wall
            for (int k = 20; k < 580; k+=20)
            {
                tileField[k].Image = LoadSpriteFromSheet(2, 3);
                NOFobjectID[k] = 2;
                NOFdata0[k] = 0;
                NOFdata1[k] = 0;
                NOFdata2[k] = 0;
                NOFx1[k] = 2;
                NOFx2[k] = 0;
                NOFx3[k] = 0;
                NOFx4[k] = 0;
                NOFy1[k] = 3;
            }

            //Bottom Wall
            for (int k = 39; k < 580; k += 20)
            {
                tileField[k].Image = LoadSpriteFromSheet(2, 1);
                NOFobjectID[k] = 2;
                NOFdata0[k] = 0;
                NOFdata1[k] = 0;
                NOFdata2[k] = 0;
                NOFx1[k] = 2;
                NOFx2[k] = 0;
                NOFx3[k] = 0;
                NOFx4[k] = 0;
                NOFy1[k] = 1;
            }


        }

        private void createLevelTile(int x, int y, int tileID)
        {
            // Creates one level tile and remembers it's hash value so it can be identified
            PictureBox tile = new PictureBox();
            tile.Name = "levelTile" + tileID;
            levelTileHashes.Add(tile.GetHashCode(), tileID);
            tile.Size = new Size(20, 20);
            tile.BackColor = Color.White;
            tile.Location = new Point(x, y);
            tile.BringToFront();
            tile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnLevelTileClick);
            tileField[tileID] = tile;
            this.Controls.Add(tile);
        }

        private void OnLevelTileClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //Delete object
                tileField[levelTileHashes[sender.GetHashCode()]].Image = null;
                NOFobjectID[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFdata0[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFdata1[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFdata2[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFx1[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFx2[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFx3[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFx4[levelTileHashes[sender.GetHashCode()]] = 0;
                NOFy1[levelTileHashes[sender.GetHashCode()]] = 0;
            }
            else if (e.Button == MouseButtons.Left)
            {
                //Put selected object at place
                tileField[levelTileHashes[sender.GetHashCode()]].Image = aSelected.Image;
                NOFobjectID[levelTileHashes[sender.GetHashCode()]] = (byte)objectID;
                NOFdata0[levelTileHashes[sender.GetHashCode()]] = (byte)data0;
                NOFdata1[levelTileHashes[sender.GetHashCode()]] = (byte)data1;
                NOFdata2[levelTileHashes[sender.GetHashCode()]] = (byte)data2;
                NOFx1[levelTileHashes[sender.GetHashCode()]] = (byte)x1;
                NOFx2[levelTileHashes[sender.GetHashCode()]] = (byte)x2;
                NOFx3[levelTileHashes[sender.GetHashCode()]] = (byte)x3;
                NOFx4[levelTileHashes[sender.GetHashCode()]] = (byte)x4;
                NOFy1[levelTileHashes[sender.GetHashCode()]] = (byte)y1;
            }
        }

        private void wallTileR_Click(object sender, EventArgs e)
        {
            if (wallX > 13) return;
            wallX++;
            RenderWallTile();
        }

        private void wallTileL_Click(object sender, EventArgs e)
        {
            if (wallX < 2) return;
            wallX--;
            RenderWallTile();
        }

        private void wallUp_Click(object sender, EventArgs e)
        {
            if (wallY < 2) return;
            wallY--;
            RenderWallTile();
        }

        private void wallDown_Click(object sender, EventArgs e)
        {
            if (wallY > 6) return;
            wallY++;
            RenderWallTile();
        }

        private void RenderWallTile()
        {
            wallTile.Image = LoadSpriteFromSheet(wallX, wallY);
        }

        private void kye1_Click(object sender, EventArgs e)
        {
            aSelected.Image = kye[0];
            objectID = 1;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void diamond1_Click(object sender, EventArgs e)
        {
            aSelected.Image = diamond[0];
            objectID = 10;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void box1_Click(object sender, EventArgs e)
        {
            aSelected.Image = boxes[0];
            objectID = 4;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void box2_Click(object sender, EventArgs e)
        {
            aSelected.Image = boxes[1];
            objectID = 4;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void ghostbox1_Click(object sender, EventArgs e)
        {
            aSelected.Image = ghostboxes[0];
            objectID = 6;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void ghostbox2_Click(object sender, EventArgs e)
        {
            aSelected.Image = ghostboxes[1];
            objectID = 6;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squarep1_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaremovebox[0];
            objectID = 3;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squarep2_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaremovebox[1];
            objectID = 3;
            data0 = 0;
            data1 = 1;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squarep3_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaremovebox[2];
            objectID = 3;
            data0 = 0;
            data1 = 2;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squarep4_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaremovebox[3];
            objectID = 3;
            data0 = 0;
            data1 = 3;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlep1_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlemovebox[0];
            objectID = 3;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlep2_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlemovebox[1];
            objectID = 3;
            data0 = 1;
            data1 = 1;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlep3_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlemovebox[2];
            objectID = 3;
            data0 = 1;
            data1 = 2;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlep4_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlemovebox[3];
            objectID = 3;
            data0 = 1;
            data1 = 3;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void pusher1_Click(object sender, EventArgs e)
        {
            aSelected.Image = pushers[0];
            objectID = 9;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void pusher2_Click(object sender, EventArgs e)
        {
            aSelected.Image = pushers[1];
            objectID = 9;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void pusher3_Click(object sender, EventArgs e)
        {
            aSelected.Image = pushers[2];
            objectID = 9;
            data0 = 2;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void pusher4_Click(object sender, EventArgs e)
        {
            aSelected.Image = pushers[3];
            objectID = 9;
            data0 = 3;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squaregen1_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaregenerators[0];
            objectID = 7;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squaregen2_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaregenerators[1];
            objectID = 7;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squaregen3_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaregenerators[2];
            objectID = 7;
            data0 = 2;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void squaregen4_Click(object sender, EventArgs e)
        {
            aSelected.Image = squaregenerators[3];
            objectID = 7;
            data0 = 3;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlegen1_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlegenerators[0];
            objectID = 7;
            data0 = 0;
            data1 = 1;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlegen2_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlegenerators[1];
            objectID = 7;
            data0 = 1;
            data1 = 1;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlegen3_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlegenerators[2];
            objectID = 7;
            data0 = 2;
            data1 = 1;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void circlegen4_Click(object sender, EventArgs e)
        {
            aSelected.Image = circlegenerators[3];
            objectID = 7;
            data0 = 3;
            data1 = 1;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void destroyer_Click(object sender, EventArgs e)
        {
            aSelected.Image = destroyers[0];
            objectID = 5;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void magnetns_Click(object sender, EventArgs e)
        {
            aSelected.Image = magnets[0];
            objectID = 0x0B;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void magnetew_Click(object sender, EventArgs e)
        {
            aSelected.Image = magnets[1];
            objectID = 0x0B;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void rotateclock_Click(object sender, EventArgs e)
        {
            aSelected.Image = rotators[0];
            objectID = 0x0C;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        private void rotatecount_Click(object sender, EventArgs e)
        {
            aSelected.Image = rotators[1];
            objectID = 0x0C;
            data0 = 1;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        //Timers x1-x4 data is float
        private void timer0_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[0];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0xCD;
            x2 = 0xCC;
            x3 = 0xCC;
            x4 = 0x3D;
            y1 = 0;
        }

        private void timer1_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[1];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0xCD;
            x2 = 0xCC;
            x3 = 0x8C;
            x4 = 0x3F;
            y1 = 0;
        }

        private void timer2_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[2];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x66;
            x2 = 0x66;
            x3 = 0x06;
            x4 = 0x40;
            y1 = 0;
        }

        private void timer3_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[3];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x66;
            x2 = 0x66;
            x3 = 0x46;
            x4 = 0x40;
            y1 = 0;
        }

        private void timer4_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[4];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x33;
            x2 = 0x33;
            x3 = 0x83;
            x4 = 0x40;
            y1 = 0;
        }

        private void timer5_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[5];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x33;
            x2 = 0x33;
            x3 = 0xA3;
            x4 = 0x40;
            y1 = 0;
        }

        private void timer6_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[6];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x33;
            x2 = 0x33;
            x3 = 0xC3;
            x4 = 0x40;
            y1 = 0;
        }

        private void timer7_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[7];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x33;
            x2 = 0x33;
            x3 = 0xE3;
            x4 = 0x40;
            y1 = 0;
        }

        private void timer8_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[8];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x9A;
            x2 = 0x99;
            x3 = 0x01;
            x4 = 0x41;
            y1 = 0;
        }

        private void timer9_Click(object sender, EventArgs e)
        {
            aSelected.Image = timers[9];
            objectID = 0x0D;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0x9A;
            x2 = 0x99;
            x3 = 0x11;
            x4 = 0x41;
            y1 = 0;
        }

        private void wallTile_Click(object sender, EventArgs e)
        {
            aSelected.Image = wallTile.Image;
            objectID = 2;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = wallX;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = wallY;
        }

        private void saveLevelBTN_Click(object sender, EventArgs e)
        {
            LVLname = textBox4.Text;
            LVLintro = textBox3.Text;
            LVLhint = textBox1.Text;
            LVLwin = textBox2.Text;
            LVLid = textBox5.Text;

            if (LVLname.Length < 1 || LVLintro.Length < 1 || LVLhint.Length < 1 || LVLwin.Length < 1) return;

            if (LVLid.Length < 1 || LVLid.Length > 2) return;

            levelID = int.Parse(LVLid);
            if (levelID < 0) return;
            if (levelID == 1) tutorialLevel = true;
            else tutorialLevel = false;

            if (levelID < 10) MyWriteFile("0" + levelID.ToString() + "-" + stars + "_" + LVLname.ToLower());
            else MyWriteFile(levelID.ToString() + "-" + stars + "_" + LVLname.ToLower());

        }

        private void ReRenderLevel()
        {

            //Replace all objects with the ones loaded in memory
            for (int i = 0; i < 600; i++)
            {
                //Clear the field first
                tileField[i].Image = null;

                switch (NOFobjectID[i])
                {
                    case 0: break;
                    case 1: tileField[i].Image = kye[0]; break;
                    case 2: tileField[i].Image = LoadSpriteFromSheet(NOFx1[i], NOFy1[i]); break;
                    case 3:
                        if (NOFdata0[i] == 0) tileField[i].Image = squaremovebox[NOFdata1[i]];
                        else tileField[i].Image = circlemovebox[NOFdata1[i]];
                        break;
                    case 4: tileField[i].Image = boxes[NOFdata0[i]]; break;
                    case 5: tileField[i].Image = destroyers[0]; break;
                    case 6: tileField[i].Image = ghostboxes[NOFdata0[i]]; break;
                    case 7:
                        if (NOFdata1[i] == 0) tileField[i].Image = squaregenerators[NOFdata1[i]];
                        else tileField[i].Image = circlegenerators[NOFdata1[i]];
                        break;
                    case 9: tileField[i].Image = pushers[NOFdata0[i]]; break;
                    case 10: tileField[i].Image = diamond[0]; break;
                    case 11: tileField[i].Image = magnets[NOFdata0[i]]; break;
                    case 12: tileField[i].Image = rotators[NOFdata0[i]]; break;
                    case 13:
                        if (NOFx3[i] == 0xCC) tileField[i].Image = timers[0];
                        if (NOFx3[i] == 0x8C) tileField[i].Image = timers[1];
                        if (NOFx3[i] == 0x06) tileField[i].Image = timers[2];
                        if (NOFx3[i] == 0x46) tileField[i].Image = timers[3];
                        if (NOFx3[i] == 0x83) tileField[i].Image = timers[4];
                        if (NOFx3[i] == 0xA3) tileField[i].Image = timers[5];
                        if (NOFx3[i] == 0xC3) tileField[i].Image = timers[6];
                        if (NOFx3[i] == 0xE3) tileField[i].Image = timers[7];
                        if (NOFx3[i] == 0x01) tileField[i].Image = timers[8];
                        if (NOFx3[i] == 0x11) tileField[i].Image = timers[9];
                        break;
                    default:
                        MessageBox.Show("Couldn't load object with ID of: " + NOFobjectID[i]);
                        break;
                }
            }
        }

        private void MyReadFile(string fileName)
        {
            //Read file structure
            byte[] data = new byte[10604];
            data = File.ReadAllBytes(fileName);

            // Read Level Field
            for (int objects = 0; objects < 600; objects++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (j % 16 == 4) NOFobjectID[objects] = data[j + 16 * objects];
                    else if (j % 16 == 5) NOFdata0[objects] = data[j + 16 * objects];
                    else if (j % 16 == 6) NOFdata1[objects] = data[j + 16 * objects];
                    else if (j % 16 == 7) NOFdata2[objects] = data[j + 16 * objects];
                    else if (j % 16 == 8) NOFx1[objects] = data[j + 16 * objects];
                    else if (j % 16 == 9) NOFx2[objects] = data[j + 16 * objects];
                    else if (j % 16 == 10) NOFx3[objects] = data[j + 16 * objects];
                    else if (j % 16 == 11) NOFx4[objects] = data[j + 16 * objects];
                    else if (j % 16 == 12) NOFy1[objects] = data[j + 16 * objects];
                    else data[j + 16 * objects] = 0x00;
                }
            }

            //Read Level Name
            LVLname = string.Empty;
            LVLintro = string.Empty;
            LVLhint = string.Empty;
            LVLwin = string.Empty;

            for (int k = 0; k < data[9600]; k++)
            {
                LVLname += Convert.ToChar(data[9601 + k]).ToString();
            }

            //Write Level Intro Text

            for (int k = 0; k < data[9851]; k++)
            {
                LVLintro += Convert.ToChar(data[9852 + k]).ToString();
            }

            //Write Level Hint

            for (int k = 0; k < data[10102]; k++)
            {
                LVLhint += Convert.ToChar(data[10103 + k]).ToString();
            }

            //Write Level Win MSG

            for (int k = 0; k < data[10353]; k++)
            {
                LVLwin += Convert.ToChar(data[10354 + k]).ToString();
            }

            textBox4.Text = LVLname;
            textBox3.Text = LVLintro;
            textBox1.Text = LVLhint;
            textBox2.Text = LVLwin;
            textBox5.Text = LVLid;

            //stars = int.Parse(Between(fileName, "-", "_"));

            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            radioButton4.Checked = false;

            switch (stars)
            {
                case 1: radioButton1.Checked = true; break;
                case 2: radioButton2.Checked = true; break;
                case 3: radioButton3.Checked = true; break;
                case 4: radioButton4.Checked = true; break;
            }

            ReRenderLevel();
        }

        private void MyWriteFile(string fileName)
        {
            //Create empty file structure
            byte[] data = new byte[10604];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }


            // Write Level Field
            for (int objects = 0; objects < 600; objects++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (j % 16 == 4) data[j + 16 * objects] = NOFobjectID[objects];
                    else if (j % 16 == 5) data[j + 16 * objects] = NOFdata0[objects];
                    else if (j % 16 == 6) data[j + 16 * objects] = NOFdata1[objects];
                    else if (j % 16 == 7) data[j + 16 * objects] = NOFdata2[objects];
                    else if (j % 16 == 8) data[j + 16 * objects] = NOFx1[objects];
                    else if (j % 16 == 9) data[j + 16 * objects] = NOFx2[objects];
                    else if (j % 16 == 10) data[j + 16 * objects] = NOFx3[objects];
                    else if (j % 16 == 11) data[j + 16 * objects] = NOFx4[objects];
                    else if (j % 16 == 12) data[j + 16 * objects] = NOFy1[objects];
                    else data[j + 16 * objects] = 0x00;
                }
            }

            //Write Level Name

            data[9600] = (byte)LVLname.Length;

            for (int k = 0; k < data[9600]; k++)
            {
                data[9601 + k] = Convert.ToByte(LVLname.Substring(k, 1).ToCharArray()[0]);
            }

            //Write Level Intro Text

            data[9851] = (byte)LVLintro.Length;

            for (int k = 0; k < data[9851]; k++)
            {
                data[9852 + k] = Convert.ToByte(LVLintro.Substring(k, 1).ToCharArray()[0]);
            }

            //Write Level Hint

            data[10102] = (byte)LVLhint.Length;

            for (int k = 0; k < data[10102]; k++)
            {
                data[10103 + k] = Convert.ToByte(LVLhint.Substring(k, 1).ToCharArray()[0]);
            }

            //Write Level Win MSG

            data[10353] = (byte)LVLwin.Length;

            for (int k = 0; k < data[10353]; k++)
            {
                data[10354 + k] = Convert.ToByte(LVLwin.Substring(k, 1).ToCharArray()[0]);
            }


            File.WriteAllBytes(fileName + (tutorialLevel ? ".kyt" : ".kyl"), data);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            stars = 1;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            stars = 2;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            stars = 3;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            stars = 4;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "kyl";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileloc = openFileDialog1.FileName;
                MyReadFile(fileloc);
            }
        }
        
    }
}
