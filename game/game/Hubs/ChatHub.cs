using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Timers;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        //TODO: Need to use global variable on database

        //This version recreates gridArray everytime client access hub
        public int[,] gridArray = new int[12, 12];
        public int[] array1D = new int[144];
        public string arrayString;
        SQLCode sql = new SQLCode();
        DBConnect db = new DBConnect();
        Timer timer = new Timer(2000);

        //Initiliaze grid to glider formation
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            //Get string from DB
            string viewString = sql.viewGrid();
            arrayString = db.readDB(viewString);

            //Convert DB string to array, initiliaze to glider, convert array to DB string
            StringtoArray(arrayString);
            GliderArray();
            ArraytoString(gridArray);
            
            //Update string in db
            string updateString = sql.updateDbString(arrayString);
            db.updateDB(updateString);

            //conver to 1d Array and update all clients
            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);

            return base.OnConnected();
        }

        private Task HandleTimer()
        {
            throw new NotImplementedException();
        }

        //Client calls to execute next step in game logic 
        public void Next()
        {
            string name = Context.User.Identity.Name;

            //Get string from DB
            string viewString = sql.viewGrid();
            arrayString = db.readDB(viewString);

            //Convert DB string to array, update next step, convert array to DB string
            StringtoArray(arrayString);
            Update();
            ArraytoString(gridArray);

            //Update string in db
            string updateString = sql.updateDbString(arrayString);
            db.updateDB(updateString);

            //Convert to 1d array and update all clients
            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);
        }

        //Client calls when Hit Test is triggered 
        public void Change(int row, int col)
        {
            string name = Context.User.Identity.Name;
            //Get string from DB
            string viewString = sql.viewGrid();
            arrayString = db.readDB(viewString);

            //Convert DB string to array, change specified grid cell, convert array to DB string
            StringtoArray(arrayString);
            gridArray[row, col] = 1;
            ArraytoString(gridArray);

            //Update string in db
            string updateString = sql.updateDbString(arrayString);
            db.updateDB(updateString);

            //Convert to 1d array and update all clients
            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);

        }

        public void Start()
        {
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Update);
            timer.Start();
           
        }

        

        //Converts inputed string to gridArray 2 dimensional array
        public void StringtoArray(string str)
        {
            int row = 0;
            int col = 0;
            int count = 1;
            foreach (char c in str)
            {
                gridArray[row, col] = (int)Char.GetNumericValue(c);
                if (count % 12 == 0)
                {
                    row++;
                    col = 0;
                }
                else
                {
                    col++;
                }
                count++;
            }
        }

        //Converts inputed 2D array to string for database
        public void ArraytoString(int[,] arr)
        {
            arrayString = "";
            for (int r = 0; r < 12; r++)
            {
                for (int c = 0; c < 12; c++)
                {
                    arrayString += arr[r, c];
                }
            }
        }

        //Converts inputed 2d array to gridArray 1d array
        public void Array2dtoArray1d(int[,] arr)
        {
            int count = 0;
            for (int r = 0; r < 12; r++)
            {
                for (int c = 0; c < 12; c++)
                {
                    array1D[count] = gridArray[r, c];
                    count++;
                }
            }
        }

        public void GliderArray()
        {
            //Initializes neighborhood to glider formation
            for (int r = 0; r < 12; r++) {
                for (int c = 0; c < 12; c++) {
                    if ((r == 6 && (c == 4 || c == 5 || c == 6)) ||
                        (r == 5 && c == 6) ||
                        (r == 4 && c == 5)) {
                        gridArray[r, c] = 1;
                    }
                    else {
                        gridArray[r, c] = 0;
                    }
                }
            }

        }

        //Checks neighborhood and brings alive/kills based on game logic
        public void Update()
        {
            int[,] newArray = gridArray.Clone() as int[,];

            for (int gridRow = 4; gridRow < 11; gridRow++)
            {
                for (int gridCol = 4; gridCol < 11; gridCol++)
                {
                    int count = CheckNeighbors(gridRow, gridCol);

                    if (count == 3)
                        newArray[gridRow, gridCol] = 1;

                    if (count > 3 || count < 2)
                        newArray[gridRow, gridCol] = 0;

                }
            }

            gridArray = newArray.Clone() as int[,];
        }

        //Checks neighborhood and brings alive/kills based on game logic
        public void Update(object  sender, ElapsedEventArgs e)
        {
            Update();
        }


        //Keeps count on number of neighbors cell has
        public int CheckNeighbors(int r, int c)
        {
            int count = 0;
            //check if on edge
            if (r != 0 && r != 12 && c != 0 && c != 12)
            {
                //starting in upper left corner, checking clock-wise
                if (IsAlive(r - 1, c - 1)) count++;
                if (IsAlive(r - 1, c)) count++;
                if (IsAlive(r - 1, c + 1)) count++;
                if (IsAlive(r, c + 1)) count++;
                if (IsAlive(r + 1, c + 1)) count++;
                if (IsAlive(r + 1, c)) count++;
                if (IsAlive(r + 1, c - 1)) count++;
                if (IsAlive(r, c - 1)) count++;
            }
            return count;
        }

        //Checks if cell is alive
        public bool IsAlive(int r, int c)
        {
            if (gridArray[r, c] == 1)
                return true;      
            else  
                return false;
            
        }





    }//end class
}