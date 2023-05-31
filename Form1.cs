using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace AmazingKyeEditor
{
    public partial class Form1 : Form
    {
        public static Form1 instance;

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
        private Dictionary<int, int> wallHashes = new Dictionary<int, int>();
        private string LVLname;
        private string LVLintro;
        private string LVLhint;
        private string LVLwin;
        private string LVLid;
        private bool tutorialLevel;
        private int levelID;
        private int stars = 1;
        private bool AutoWallMode;
        private Image currentAutoWallTex;
        private static Bitmap spritesheet;
        private bool levelIsLoading;

        private Image LoadSpriteFromSheet(int x, int y)
        {
            x--;
            y--;
            return spritesheet.Clone(new Rectangle(x * 20, y * 20, 20, 20), spritesheet.PixelFormat);
        }

        public Form1()
        {
            Form1.instance = this;
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

            Point manualWallsDefaultLoc = new Point(875, 285);
            //Load manual wall tiles
            int manualX = 1;
            int manualY = 1;
            for (int i = 0; i < 14 * 20; i+=20)
            {
                for (int j = 0; j < 7 * 20; j += 20)
                {
                    manualX = i / 20 + 1;
                    manualY = j / 20 + 1;
                    createManualWallTile(manualWallsDefaultLoc.X + i, manualWallsDefaultLoc.Y + j, manualX, manualY);
                }
            }

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

            currentAutoWallTex = LoadSpriteFromSheet(2, 2);

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

        private void createManualWallTile(int x, int y, int manualX, int manualY)
        {
            PictureBox tile = new PictureBox();
            tile.Size = new Size(20, 20);
            tile.BackColor = Color.White;
            tile.Location = new Point(x, y);
            tile.BringToFront();
            tile.Image = LoadSpriteFromSheet(manualX, manualY);
            tile.MouseClick += (sender, e) => wallTile_Click(sender, e, manualX, manualY);
            this.Controls.Add(tile);
        }

        private void createLevelTile(int x, int y, int tileID)
        {
            // Creates one level tile and remembers it's hash value so it can be identified
            PictureBox tile = new PictureBox();
            tile.Name = "levelTile" + tileID;
            wallHashes.Add(tile.GetHashCode(), tileID);
            tile.Size = new Size(20, 20);
            tile.BackColor = Color.White;
            tile.Location = new Point(x, y);
            tile.BringToFront();
            tile.MouseMove += new MouseEventHandler(this.OnLevelTileClick);
            tileField[tileID] = tile;
            tile.Tag = "AutoUpdateTile";
            this.Controls.Add(tile);
        }

        [DllImport("user32.dll")]
        private static extern bool SetCapture(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private bool BorderCheckPass(int root, bool bypass = false)
        {
            if (root < 20) return !bypass ? false : !AutoWallMode;
            if (root >= 580) return !bypass ? false : !AutoWallMode;
            if (root % 20 == 0) return !bypass ? false : !AutoWallMode;
            if (root % 20 == 19) return !bypass ? false : !AutoWallMode;

            return true;
        }

        private void OnLevelTileClick(object sender, MouseEventArgs e)
        {
            if (levelIsLoading) return;
            if (Form2.ActiveSelf) return;
            //This code checks if you're hovering a tile from the field
            Control control = sender as Control;
            if (control != null && control.Tag != null && control.Tag.ToString() == "AutoUpdateTile")
            {
                // Capture the mouse to receive global mouse events
                SetCapture(Handle);

                // Release the captured mouse
                ReleaseCapture();
            }

            //Prevents a crash with the wall borders
            if (!wallHashes.ContainsKey(sender.GetHashCode())) return; 

            if (e.Button == MouseButtons.Right)
            {
                if (!BorderCheckPass(wallHashes[sender.GetHashCode()]))
                {
                    ReplaceBorderWallPiece(wallHashes[sender.GetHashCode()]);
                    return;
                }
                if (AutoWallMode)
                {
                    GetNeighborInfo(wallHashes[sender.GetHashCode()]);
                }
                //Delete object
                tileField[wallHashes[sender.GetHashCode()]].Image = null;
                NOFobjectID[wallHashes[sender.GetHashCode()]] = 0;
                NOFdata0[wallHashes[sender.GetHashCode()]] = 0;
                NOFdata1[wallHashes[sender.GetHashCode()]] = 0;
                NOFdata2[wallHashes[sender.GetHashCode()]] = 0;
                NOFx1[wallHashes[sender.GetHashCode()]] = 0;
                NOFx2[wallHashes[sender.GetHashCode()]] = 0;
                NOFx3[wallHashes[sender.GetHashCode()]] = 0;
                NOFx4[wallHashes[sender.GetHashCode()]] = 0;
                NOFy1[wallHashes[sender.GetHashCode()]] = 0;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                if (!BorderCheckPass(wallHashes[sender.GetHashCode()])) return;
                if (NOFobjectID[wallHashes[sender.GetHashCode()]] == 0) return;
                aSelected.Image = tileField[wallHashes[sender.GetHashCode()]].Image;
                objectID = NOFobjectID[wallHashes[sender.GetHashCode()]];
                data0 = NOFdata0[wallHashes[sender.GetHashCode()]];
                data1 = NOFdata1[wallHashes[sender.GetHashCode()]];
                data2 = NOFdata2[wallHashes[sender.GetHashCode()]];
                x1 = NOFx1[wallHashes[sender.GetHashCode()]];
                x2 = NOFx2[wallHashes[sender.GetHashCode()]];
                x3 = NOFx3[wallHashes[sender.GetHashCode()]];
                x4 = NOFx4[wallHashes[sender.GetHashCode()]];
                y1 = NOFy1[wallHashes[sender.GetHashCode()]];
            }
            else if (e.Button == MouseButtons.Left)
            {
                //Bypass mode, you can place certain objects
                if (!BorderCheckPass(wallHashes[sender.GetHashCode()], true)) return;
                if (AutoWallMode)
                {
                    //Auto Wall mechanic
                    int[] xy = GetNeighborInfo(wallHashes[sender.GetHashCode()]);
                    tileField[wallHashes[sender.GetHashCode()]].Image = currentAutoWallTex;
                    NOFobjectID[wallHashes[sender.GetHashCode()]] = (byte)2;
                    NOFdata0[wallHashes[sender.GetHashCode()]] = (byte)0;
                    NOFdata1[wallHashes[sender.GetHashCode()]] = (byte)0;
                    NOFdata2[wallHashes[sender.GetHashCode()]] = (byte)0;
                    NOFx1[wallHashes[sender.GetHashCode()]] = (byte)xy[0];
                    NOFx2[wallHashes[sender.GetHashCode()]] = (byte)0;
                    NOFx3[wallHashes[sender.GetHashCode()]] = (byte)0;
                    NOFx4[wallHashes[sender.GetHashCode()]] = (byte)0;
                    NOFy1[wallHashes[sender.GetHashCode()]] = (byte)xy[1];
                }

                else
                {
                    //Put selected object at place
                    tileField[wallHashes[sender.GetHashCode()]].Image = aSelected.Image;
                    NOFobjectID[wallHashes[sender.GetHashCode()]] = (byte)objectID;
                    NOFdata0[wallHashes[sender.GetHashCode()]] = (byte)data0;
                    NOFdata1[wallHashes[sender.GetHashCode()]] = (byte)data1;
                    NOFdata2[wallHashes[sender.GetHashCode()]] = (byte)data2;
                    NOFx1[wallHashes[sender.GetHashCode()]] = (byte)x1;
                    NOFx2[wallHashes[sender.GetHashCode()]] = (byte)x2;
                    NOFx3[wallHashes[sender.GetHashCode()]] = (byte)x3;
                    NOFx4[wallHashes[sender.GetHashCode()]] = (byte)x4;
                    NOFy1[wallHashes[sender.GetHashCode()]] = (byte)y1;
                }

            }
        }

        private void ReplaceBorderWallPiece(int root)
        {
            //Right clicking on a border wall piece replaces it with what was there before
            byte[] wallTex = new byte[2];

            if (root == 0) wallTex = new byte[2] { 7, 6 };
            if (root == 19) wallTex = new byte[2] { 6, 6 };
            if (root == 580) wallTex = new byte[2] { 5, 6 };
            if (root == 599) wallTex = new byte[2] { 8, 6 };
            if (root >= 1 && root < 19) wallTex = new byte[2] { 3, 2 };
            if (root >= 581 && root < 599) wallTex = new byte[2] { 1, 2 };
            if (root >= 1 && root < 19) wallTex = new byte[2] { 3, 2 };
            if (root >= 20 && root < 580 && root % 20 == 0) wallTex = new byte[2] { 2, 3 };
            if (root >= 39 && root < 580 && root % 20 == 19) wallTex = new byte[2] { 2, 1 };

            if (checkBox2.Checked) wallTex = new byte[2] { 2, 2 };

            tileField[root].Image = LoadSpriteFromSheet(wallTex[0], wallTex[1]);
            NOFobjectID[root] = 2;
            NOFdata0[root] = 0;
            NOFdata1[root] = 0;
            NOFdata2[root] = 0;
            NOFx1[root] = wallTex[0];
            NOFx2[root] = 0;
            NOFx3[root] = 0;
            NOFx4[root] = 0;
            NOFy1[root] = wallTex[1];
        }

        private void SetNeighbor(int root)
        {
            if (BorderCheckPass(root))
            {
                if (NOFobjectID[root] != 2) return;
                int[] xy2 = GetNeighborInfo(root, null, false);
                tileField[root].Image = LoadSpriteFromSheet(xy2[0], xy2[1]);
                NOFobjectID[root] = (byte)2;
                NOFdata0[root] = (byte)0;
                NOFdata1[root] = (byte)0;
                NOFdata2[root] = (byte)0;
                NOFx1[root] = (byte)xy2[0];
                NOFx2[root] = (byte)0;
                NOFx3[root] = (byte)0;
                NOFx4[root] = (byte)0;
                NOFy1[root] = (byte)xy2[1];
            }
        }

        private int[] GetNeighborInfo(int root, bool[] neighbors = null, bool setMoreNeighbors = true)
        {
            if (root - 20 - 1 < 0) return new int[2] { 2, 2 };
            if (root + 20 + 1 >= 600) return new int[2] { 2, 2 };

            neighbors = new bool[8];
            neighbors[0] = AllowAutowallObjectCheck(NOFobjectID[root - 20 - 1]);
            neighbors[1] = AllowAutowallObjectCheck(NOFobjectID[root -  0 - 1]);
            neighbors[2] = AllowAutowallObjectCheck(NOFobjectID[root + 20 - 1]);
            neighbors[3] = AllowAutowallObjectCheck(NOFobjectID[root + 20 - 0]);
            neighbors[4] = AllowAutowallObjectCheck(NOFobjectID[root + 20 + 1]);
            neighbors[5] = AllowAutowallObjectCheck(NOFobjectID[root +  0 + 1]);
            neighbors[6] = AllowAutowallObjectCheck(NOFobjectID[root - 20 + 1]);
            neighbors[7] = AllowAutowallObjectCheck(NOFobjectID[root - 20 + 0]);

            // Neighbor debug
            /*
            if (setMoreNeighbors)
            {
                
                System.Diagnostics.Debug.WriteLine("@@@@@@@@@@@");

                System.Diagnostics.Debug.WriteLine((neighbors[0].ToString() + neighbors[1] + neighbors[2]).Replace("alse", "").Replace("rue", ""));
                System.Diagnostics.Debug.WriteLine((neighbors[7].ToString() + "X" + neighbors[3]).Replace("alse", "").Replace("rue", ""));
                System.Diagnostics.Debug.WriteLine((neighbors[6].ToString() + neighbors[5] + neighbors[4]).Replace("alse", "").Replace("rue", ""));

                System.Diagnostics.Debug.WriteLine("@@@@@@@@@@@");
                 
            }
            */

            int[] xy = null;
            LoadWallAccordingtoNeighbors(out xy, neighbors[0], neighbors[1], neighbors[2], neighbors[3], neighbors[4], neighbors[5], neighbors[6], neighbors[7]);

            if (setMoreNeighbors)
            {
                currentAutoWallTex = LoadSpriteFromSheet(xy[0], xy[1]);
                SetNeighbor(root - 20 - 1);
                SetNeighbor(root - 0 - 1);
                SetNeighbor(root + 20 - 1);
                SetNeighbor(root + 20 - 0);
                SetNeighbor(root + 20 + 1);
                SetNeighbor(root + 0 + 1);
                SetNeighbor(root - 20 + 1);
                SetNeighbor(root - 20 + 0);
            }

            return xy;
        }

        private bool AllowAutowallObjectCheck(int neighbor)
        {
            if (blendingCheckbox.Checked) return neighbor == 2 || neighbor == 3 || neighbor == 4 || neighbor == 5 || neighbor == 6 || neighbor == 7 || neighbor == 9 || neighbor == 0x0B || neighbor == 0x0C || neighbor == 0x0D;
            else return neighbor == 2;
        }

        private void LoadWallAccordingtoNeighbors(out int[] xy, bool a, bool b, bool c, bool d, bool e, bool f, bool g, bool h)
        {
            xy = new int[2];
            xy[0] = 2;
            xy[1] = 2;

            //Fallbacks (Sphere and square tiles)
            if (!b && !d && !f && !h) { xy = new int[] { 10, 6 }; }
            if (a && b && c && d && e && f && g && h) { xy = new int[] { 2, 2 }; }

            //Edges without dots
            if (b && d && !f && h) { xy = new int[] { 2, 3 }; }
            if (b && d && f && !h) { xy = new int[] { 1, 2 }; }
            if (!b && d && f && h) { xy = new int[] { 2, 1 }; }
            if (b && !d && f && h) { xy = new int[] { 3, 2 }; }

            //Edges with singular dots
            if (!a && b && d && !f && h) { xy = new int[] { 5, 5 }; }
            if (b && !c && d && !f && h) { xy = new int[] { 6, 5 }; }
            if (!b && d && f && !g && h) { xy = new int[] { 2, 5 }; }
            if (!b && d && !e && f && h) { xy = new int[] { 3, 5 }; }
            if (b && !c && d && f && !h) { xy = new int[] { 6, 7 }; }
            if (b && d && !e && f && !h) { xy = new int[] { 8, 7 }; }
            if (!a && b && !d && f && h) { xy = new int[] { 7, 7 }; }
            if (b && !d && f && !g && h) { xy = new int[] { 9, 7 }; }

            //Ediges with double dots
            if (!a && b && !c && d && !f && h) { xy = new int[] { 1, 6 }; }
            if (!b && d && !e && f && !g && h) { xy = new int[] { 2, 6 }; }
            if (b && !c && d && !e && f && !h) { xy = new int[] { 3, 6 }; }
            if (!a && b && !d && f && !g && h) { xy = new int[] { 4, 6 }; }

            //Corners without dot
            if (!b && d && e && f && !h) { xy = new int[] { 1, 1 }; }
            if (!b && !d && f && g && h) { xy = new int[] { 3, 1 }; }
            if (a && b && !d && !f && h) { xy = new int[] { 3, 3 }; }
            if (b && c && d && !f && !h) { xy = new int[] { 1, 3 }; }

            //Corners with dot
            if (!b && d && !f && !h) { xy = new int[] { 7, 5 }; }
            if (!b && !d && f && !h) { xy = new int[] { 8, 5 }; }
            if (!b && !d && !f && h) { xy = new int[] { 9, 5 }; }
            if (b && !d && !f && !h) { xy = new int[] { 10, 5 }; }

            //Pipe horizontal and vertical pieces
            if (b && !d && f && !h) { xy = new int[] { 1, 5 }; }
            if (!b && d && !f && h) { xy = new int[] { 4, 5 }; }

            //Pipe end pieces
            if (!b && d && !e && f && !h) { xy = new int[] { 11, 5 }; }
            if (b && !c && d && !f && !h) { xy = new int[] { 11, 6 }; }
            if (!b && !d && f && !g && h) { xy = new int[] { 11, 7 }; }
            if (!a && b && !d && !f && h) { xy = new int[] { 10, 7 }; }

            //Singular dots
            if (a && b && c && d && e && f && !g && h) { xy = new int[] { 5, 6 }; }
            if (a && b && !c && d && e && f && g && h) { xy = new int[] { 6, 6 }; }
            if (a && b && c && d && !e && f && g && h) { xy = new int[] { 7, 6 }; }
            if (!a && b && c && d && e && f && g && h) { xy = new int[] { 8, 6 }; }

            //Non-singular dots
            if (a && b && c && d && !e && f && !g && h) { xy = new int[] { 1, 7 }; }
            if (!a && b && !c && d && e && f && g && h) { xy = new int[] { 2, 7 }; }
            if (a && b && !c && d && !e && f && g && h) { xy = new int[] { 3, 7 }; }
            if (!a && b && c && d && e && f && !g && h) { xy = new int[] { 4, 7 }; }
            if (!a && b && !c && d && !e && f && !g && h) { xy = new int[] { 5, 7 }; }
            
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
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
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = false;
        }

        private void LoadWall()
        {
            aSelected.Image = LoadSpriteFromSheet(wallX, wallY);
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

        private void wallTile_Click(object sender, EventArgs e, int manualX, int manualY)
        {
            wallX = manualX;
            wallY = manualY;
            LoadWall();
            AutoWallMode = false;
            autoLabel.Visible = false;
            manualLabel.Visible = true;
        }

        private void saveLevelBTN_Click(object sender, EventArgs e)
        {
            MySaveFile();
        }

        public void MySaveFile(bool raw = false)
        {
            LVLname = textBox4.Text;
            LVLintro = textBox3.Text;
            LVLhint = textBox1.Text;
            LVLwin = textBox2.Text;
            LVLid = textBox5.Text;

            int starsB4 = stars;

            if (raw)
            {
                LVLname = "Raw";
                LVLintro = "Raw Level Loader";
                LVLhint = "Try again next time";
                LVLwin = "GG :-)";
                LVLid = "98";
                stars = 0;
            }

            if (LVLname.Length < 1 || LVLintro.Length < 1 || LVLhint.Length < 1 || LVLwin.Length < 1) return;

            if (LVLid.Length < 1 || LVLid.Length > 2) return;

            levelID = int.Parse(LVLid);
            if (levelID < 0) return;
            if (levelID == 1) tutorialLevel = true;
            else tutorialLevel = false;

            if (levelID < 10) MyWriteFile("0" + levelID.ToString() + "-" + stars + "_" + LVLname.ToLower());
            else MyWriteFile(levelID.ToString() + "-" + stars + "_" + LVLname.ToLower());

            if (raw)
            {
                LVLname = textBox4.Text;
                LVLintro = textBox3.Text;
                LVLhint = textBox1.Text;
                LVLwin = textBox2.Text;
                LVLid = textBox5.Text;
                stars = starsB4;
            }

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

            //Read Level Intro Text

            for (int k = 0; k < data[9851]; k++)
            {
                LVLintro += Convert.ToChar(data[9852 + k]).ToString();
            }

            //Read Level Hint

            for (int k = 0; k < data[10102]; k++)
            {
                LVLhint += Convert.ToChar(data[10103 + k]).ToString();
            }

            //Read Level Win MSG

            for (int k = 0; k < data[10353]; k++)
            {
                LVLwin += Convert.ToChar(data[10354 + k]).ToString();
            }

            //Put Data
            textBox4.Text = LVLname;
            textBox3.Text = LVLintro;
            textBox1.Text = LVLhint;
            textBox2.Text = LVLwin;

            //Read level id and stars from file name.
            string[] uFileName = fileName.Split('\\');
            textBox5.Text = int.Parse(uFileName[uFileName.Length - 1].Substring(0, 2)).ToString();
            stars = int.Parse(uFileName[uFileName.Length - 1].Substring(3, 1));

            switch (stars)
            {
                case 1: radioButton1.Checked = true; break;
                case 2: radioButton2.Checked = true; break;
                case 3: radioButton3.Checked = true; break;
                case 4: radioButton4.Checked = true; break;
            }

            checkBox1.Checked = fileName.Substring(fileName.Length - 1, 1) == "t";

            loadingLabel.Visible = true;
            levelIsLoading = true;
            this.loadTimer.Start();
            ReRenderLevel();
        }

        public void MyReadKye(string[] level)
        {
            // Read Level Field
            int objects = 0;
            for (int j = 0; j < 30; j++)
            {
                for (int k = 0; k < 20; k++)
                {
                    byte[] obj = GetObjectFromKyeFile(level[k].ToCharArray()[j]);
                    NOFobjectID[objects] = obj[0];
                    NOFdata0[objects] = obj[1];
                    NOFdata1[objects] = obj[2];
                    NOFdata2[objects] = obj[3];
                    NOFx1[objects] = obj[4];
                    NOFx2[objects] = obj[5];
                    NOFx3[objects] = obj[6];
                    NOFx4[objects] = obj[7];
                    NOFy1[objects] = obj[8];
                    objects++;
                }
            }

            ApplyAutowallEverywhere();

            loadingLabel.Visible = true;
            levelIsLoading = true;
            this.loadTimer.Start();

            ReAddSafety(true);
            ReRenderLevel();
        }

        private byte[] GetObjectFromKyeFile(char data)
        {
            byte[] obj = new byte[9];
            switch (data)
            {
                // Empty
                case ' ':
                    break;
                case 'K':
                    // Kye
                    obj[0] = 1;
                    break;
                case '*':
                    // Diamond
                    obj[0] = 10;
                    break;
                // Walls (Will get auto-walled)
                case '1': case '2': case '3': case '4': case '5': case '6': case '7': case '8': case '9':
                    obj[0] = 2;
                    obj[4] = 2;
                    obj[8] = 2;
                    break;
                case 's':
                    // Magnet EW
                    obj[0] = 0x0B;
                    obj[4] = 2;
                    obj[8] = 2;
                    break;
                case 'S':
                    // Magnet NS
                    obj[0] = 0x0B;
                    obj[1] = 1;
                    obj[4] = 2;
                    obj[8] = 2;
                    break;
                case 'a':
                    // Rotator Clockwise
                    obj[0] = 0x0C;
                    break;
                case 'c':
                    // Rotator Counter-Clockwise
                    obj[0] = 0x0C;
                    obj[1] = 1;
                    break;
                    // Square box
                case 'b':
                    obj[0] = 4;
                    obj[4] = 2;
                    obj[8] = 2;
                    break;
                case 'B':
                    // Circle box
                    obj[0] = 4;
                    obj[1] = 1;
                    obj[4] = 2;
                    obj[8] = 2;
                    break;
                case 'e':
                    // Ghost box
                    obj[0] = 6;
                    break;
                case 'H':
                    // Destroyer
                    obj[0] = 5;
                    break;
                // Circular moving objects
                case '^':
                    obj[0] = 3;
                    obj[1] = 1;
                    obj[2] = 0;
                    break;
                case '<':
                    obj[0] = 3;
                    obj[1] = 1;
                    obj[2] = 3;
                    break;
                case 'v':
                    obj[0] = 3;
                    obj[1] = 1;
                    obj[2] = 2;
                    break;
                case '>':
                    obj[0] = 3;
                    obj[1] = 1;
                    obj[2] = 1;
                    break;
                //Square moving objects
                case 'u':
                    obj[0] = 3;
                    obj[2] = 0;
                    break;
                case 'l':
                    obj[0] = 3;
                    obj[2] = 3;
                    break;
                case 'd':
                    obj[0] = 3;
                    obj[2] = 2;
                    break;
                case 'r':
                    obj[0] = 3;
                    obj[2] = 1;
                    break;
                //Sentry moving objects
                case 'U':
                    obj[0] = 9;
                    obj[1] = 0;
                    break;
                case 'L':
                    obj[0] = 9;
                    obj[1] = 3;
                    break;
                case 'D':
                    obj[0] = 9;
                    obj[1] = 2;
                    break;
                case 'R':
                    obj[0] = 9;
                    obj[1] = 1;
                    break;
                default:
                    obj[0] = 0x0D;
                    obj[4] = 0xCD;
                    obj[5] = 0xCC;
                    obj[6] = 0xCC;
                    obj[7] = 0x3D;
                    break;
            }

            return obj;
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


            File.WriteAllBytes(fileName + (tutorialLevel || checkBox1.Checked ? ".kyt" : ".kyl"), data);
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
                if (fileloc.Substring(fileloc.Length - 3, 3).ToLower() == "kye") MessageBox.Show("KYE Loading is not supported yet (Press Load raw KYE button)");
                else MyReadFile(fileloc);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = false;
            for (int i = 0; i < 600; i++)
            {
                NOFobjectID[i] = 0;
                NOFdata0[i] = 0;
                NOFdata1[i] = 0;
                NOFdata2[i] = 0;
                NOFx1[i] = 0;
                NOFx2[i] = 0;
                NOFx3[i] = 0;
                NOFx4[i] = 0;
                NOFy1[i] = 0;
            }
            ReAddSafety();
            ReRenderLevel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            for (int i = 0; i < 600; i++)
            {
                NOFobjectID[i] = 2;
                NOFdata0[i] = 0;
                NOFdata1[i] = 0;
                NOFdata2[i] = 0;
                NOFx1[i] = 2;
                NOFx2[i] = 0;
                NOFx3[i] = 0;
                NOFx4[i] = 0;
                NOFy1[i] = 2;
            }
            ReRenderLevel();
        }

        private void ReAddSafety(bool disableOverride = false)
        {
            //Re-Add safety wall

            //Corners

            if (NOFobjectID[0] == 2 || !disableOverride)
            {
                NOFobjectID[0] = 2;
                NOFdata0[0] = 0;
                NOFdata1[0] = 0;
                NOFdata2[0] = 0;
                NOFx1[0] = 7;
                NOFx2[0] = 0;
                NOFx3[0] = 0;
                NOFx4[0] = 0;
                NOFy1[0] = 6;
            }

            if (NOFobjectID[19] == 2 || !disableOverride)
            {
                NOFobjectID[19] = 2;
                NOFdata0[19] = 0;
                NOFdata1[19] = 0;
                NOFdata2[19] = 0;
                NOFx1[19] = 6;
                NOFx2[19] = 0;
                NOFx3[19] = 0;
                NOFx4[19] = 0;
                NOFy1[19] = 6;
            }

            if (NOFobjectID[580] == 2 || !disableOverride)
            {
                NOFobjectID[580] = 2;
                NOFdata0[580] = 0;
                NOFdata1[580] = 0;
                NOFdata2[580] = 0;
                NOFx1[580] = 5;
                NOFx2[580] = 0;
                NOFx3[580] = 0;
                NOFx4[580] = 0;
                NOFy1[580] = 6;
            }

            if (NOFobjectID[599] == 2 || !disableOverride)
            {
                NOFobjectID[599] = 2;
                NOFdata0[599] = 0;
                NOFdata1[599] = 0;
                NOFdata2[599] = 0;
                NOFx1[599] = 8;
                NOFx2[599] = 0;
                NOFx3[599] = 0;
                NOFx4[599] = 0;
                NOFy1[599] = 6;
            }


            //Left wall
            for (int k = 1; k < 19; k++)
            {
                if (NOFobjectID[k] == 2 || !disableOverride)
                {
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
            }



            //Right wall
            for (int k = 581; k < 599; k++)
            {
                if (NOFobjectID[k] == 2 || !disableOverride)
                {
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
            }



            //Top Wall
            for (int k = 20; k < 580; k += 20)
            {
                if (NOFobjectID[k] == 2 || !disableOverride)
                {
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
            }

            //Bottom Wall
            for (int k = 39; k < 580; k += 20)
            {
                if (NOFobjectID[k] == 2 || !disableOverride)
                {
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
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Amazing Kye Editor by nasko222, version v1.04");
        }

        private void autoWallTile_Click(object sender, EventArgs e)
        {
            wallX = 2;
            wallY = 2;
            LoadWall();
            AutoWallMode = true;
            autoLabel.Visible = true;
            manualLabel.Visible = false;
        }

        private void loadTimer_Tick(object sender, EventArgs e)
        {
            loadingLabel.Visible = false;
            levelIsLoading = false;
            loadTimer.Stop();
        }

        private void autowallBTN_Click(object sender, EventArgs e)
        {
            ApplyAutowallEverywhere();
        }

        private void ApplyAutowallEverywhere()
        {
            for (int iroot = 0; iroot < 600; iroot++)
            {
                SetNeighbor(iroot);
            }
        }

        private void rawKyeBTN_Click(object sender, EventArgs e)
        {
            if (!Form2.ActiveSelf)
            {
                button2.Enabled = false;
                button3.Enabled = false;
                autowallBTN.Enabled = false;
                loadLevelBTN.Enabled = false;
                saveLevelBTN.Enabled = false;
                rawKyeBTN.Enabled = false;
                Form2.ActiveSelf = true;
                new Form2().Show();
            }
        }

        public void ReactivateMain()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            autowallBTN.Enabled = true;
            loadLevelBTN.Enabled = true;
            saveLevelBTN.Enabled = true;
            rawKyeBTN.Enabled = true;
        }
        
    }
}
