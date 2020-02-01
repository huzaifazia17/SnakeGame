//Hayden Foster
//ICS 
//Snake Game
//June 14, 2019

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        //list of snake segments
        IList<Rectangle> snake = new List<Rectangle>();
        //movement timer
        Timer moveTimer;
        //food rectangle
        Rectangle food;
        //scoreboard rectangle
        Rectangle scoreBoard;
        //random number
        Random rNum = new Random();

        //x speed
        int dx = 0;
        //y speed
        int dy = 0;
        //food x 
        int foodx = 0;
        //food y
        int foody = 0;
        //gridscale spacing
        const int gridScale = 25;
        //score
        int score = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Sets the text of the window to "Snake game"
            this.Text = "Snake Game";
            //Sets the height of the window to 800
            this.Height = 800;
            //Sets the height of the window to 800
            this.Width = 800;
            //sets minimizebox to false
            this.MaximizeBox = false;
            //centers form
            this.CenterToScreen();
            //events for drawing to screen and keydown events
            this.DoubleBuffered = true;

            //makes scoreboard rectangle
            scoreBoard = new Rectangle(0, 0, ClientSize.Width, 50);
            //adds a snake rectangle to list
            snake.Add(new Rectangle(0, 50, 25, 25));
            //makes food rect
            food = new Rectangle(100, 150, 25, 25);

            //makes paint event
            this.Paint += Form1_Paint;
            //makes keydown event
            this.KeyDown += Form1_KeyDown;
            //Creates a timer
            moveTimer = new Timer();
            //Creates method for timer
            moveTimer.Tick += MoveTimer_Tick;
            //Sets the timer interval to 12fps
            moveTimer.Interval = 1000 / 12;
            //Starts timer
            moveTimer.Start();
        }

        //keydown event to check keys
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //checks if up arrow is pressed
            if (e.KeyCode == Keys.Up)
            {
                //if dy is not 25
                if (dy != 25)
                {
                    //sets dx to 0
                    dx = 0;
                    //sets dy to -25
                    dy = -25;
                }
            }
            //checks if up arrow is pressed
            if (e.KeyCode == Keys.Down)
            {
                //if dy is not -25
                if (dy != -25)
                {
                    //sets dx to 0
                    dx = 0;
                    //sets dy to 25
                    dy = 25;
                }
            }
            //checks if up arrow is pressed
            if (e.KeyCode == Keys.Right)
            {
                //if dx is not -25
                if (dx != -25)
                {
                    //sets dx to 25
                    dx = 25;
                    //sets dy to 0
                    dy = 0;
                }
            }
            //checks if up arrow is pressed
            if (e.KeyCode == Keys.Left)
            {
                //if dx is not 25
                if (dx != 25)
                {
                    //sets dx to -25
                    dx = -25;
                    //sets dy to 
                    dy = 0;
                }
            }
        }

        //timer method
        private void MoveTimer_Tick(object sender, EventArgs e)
        {
            //declares a temp rectangle
            Rectangle tempRec;
            //runs through snake list
            for (int i = snake.Count - 1; i >= 0; i--)
                {
                //if i is 0
                if (i == 0)
                {
                    //sets temprec to snake
                    tempRec = snake[0];
                    //adds dx to temprec's x
                    tempRec.X += dx;
                    //adds dy to temprec's y
                    tempRec.Y += dy;
                    //sets snake to temprec
                    snake[0] = tempRec;
                }
                //otherwise
                else
                {
                    //sets temprec to snake
                    tempRec = snake[i];
                    //sets temprec's x to previous snake's x
                    tempRec.X = snake[i-1].X;
                    //sets temprec's y to previous snake's y
                    tempRec.Y = snake[i-1].Y;
                    //sets snake to temprec
                    snake[i] = tempRec;
                }

            }

            //runs through snake list
            for (int i = 0; i < snake.Count; i++)
            {
                //if snake intersects with food rectangle
                if (snake[i].IntersectsWith(food))
                {
                    //play sound
                    SystemSounds.Hand.Play();
                    //createFood method
                    createFood();
                    //add one to score
                    score++;
                    //add new rectangle to list
                    snake.Add(new Rectangle(snake[snake.Count-1].X -dx, snake[snake.Count-1].Y -dy, 25, 25));
                }

                //checks if snake is off screen
                if (snake[i].X > ClientSize.Width || snake[i].X < 0 || snake[i].Y > ClientSize.Height || snake[i].Y < 50)
                {
                    //death method
                    death();
                }

                //rund through snake list
                for (int j = 0; j < snake.Count; j++)
                {
                    //if j is 1
                    if (j == i)
                    {
                        //breaks
                        break;
                    }
                    //if snake intersects with itself
                    else if (snake[i].IntersectsWith(snake[j]))
                    {
                        //death method
                        death();
                    }
                }
            }
            //refreshes screen
            this.Invalidate();
        }

        //death method
        private void death()
        {
            //stops timer
            moveTimer.Stop();
            //shows gameover box
            MessageBox.Show("Game Over!\nYour score is: " + score);
            //ends game
            Application.Exit();
        }

        //createfood method
        private void createFood()
        {
            //sets foodx to the screen width divided by gridscale
            foodx = (ClientSize.Width - snake[0].Width) / gridScale;
            //sets foody to the screen height divided by gridscale
            foody = (ClientSize.Height - snake[0].Height) / gridScale;
            //sets food's x to a random x
            food.X = rNum.Next(0, foodx) * gridScale;
            //sets food's y to a random y
            food.Y = rNum.Next(0, foody) * gridScale;
            
        }

        //paint method
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //runs through snake list
            for (int i = 0; i < snake.Count; i++)
            {
                //fills snake with dark green color
                e.Graphics.FillRectangle(Brushes.DarkGreen, snake[i]);
            }
            //fills food red
            e.Graphics.FillRectangle(Brushes.DarkRed, food);
            //fills scoreboard silver
            e.Graphics.FillRectangle(Brushes.Silver, scoreBoard);
            //draws the score on top left of screen
            e.Graphics.DrawString("Score: " + Convert.ToString(score), new Font("Arial", 16), new SolidBrush(Color.Black), 0, 15);
        }
    }
}
