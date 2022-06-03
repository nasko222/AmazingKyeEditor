using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
        private int[] NOFobjectID;
        private int[] NOFdata0;
        private int[] NOFdata1;
        private int[] NOFdata2;
        private int[] NOFx1;
        private int[] NOFx2;
        private int[] NOFx3;
        private int[] NOFx4;
        private int[] NOFy1;
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
        private Dictionary<int, int> levelTileHashes = new Dictionary<int, int>();

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
            trashBack.Image = LoadSpriteFromSheet(4, 1);
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
            tile.Click += new System.EventHandler(this.OnLevelTileClick);
            this.Controls.Add(tile);
        }

        private void OnLevelTileClick(object sender, EventArgs e)
        {
            MessageBox.Show(levelTileHashes[sender.GetHashCode()].ToString());
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

        private void trashBack_Click(object sender, EventArgs e)
        {
            aSelected.Image = LoadSpriteFromSheet(4, 1);
            objectID = 0;
            data0 = 0;
            data1 = 0;
            data2 = 0;
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            y1 = 0;
        }

        
    }
}
