using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
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

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;
            string viewString = sql.viewGrid();
            arrayString = db.readDB(viewString);

            StringtoArray(arrayString);
            GliderArray();
            ArraytoString(gridArray);
            string updateString = sql.updateDbString(arrayString);
            db.updateDB(updateString);

            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);


            return base.OnConnected();
        }

        public void Send()
        {

            string name = Context.User.Identity.Name;
            string viewString = sql.viewGrid();
            arrayString = db.readDB(viewString);

            StringtoArray(arrayString);
            Update();
            ArraytoString(gridArray);
            string updateString = sql.updateDbString(arrayString);
            db.updateDB(updateString);

            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);
        }

        //Client calls when Hit Test is triggered 
        public void CellClicked(int row, int col)
        {
            string name = Context.User.Identity.Name;
            string viewString = sql.viewGrid();
            arrayString = db.readDB(viewString);

            StringtoArray(arrayString);
            gridArray[row, col] = 1;
            ArraytoString(gridArray);
            string updateString = sql.updateDbString(arrayString);
            db.updateDB(updateString);

            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);
            gridArray[row, col] = 1;

            Array2dtoArray1d(gridArray);
            Clients.All.updateArrayOnPage(array1D);
        }


        public void GliderArray()
        {
            //Initializes neighborhood to glider formation
            for (int r = 0; r < 12; r++)
            {
                for (int c = 0; c < 12; c++)
                {
                    if ((r == 6 && (c == 4 || c == 5 || c == 6)) ||
                        (r == 5 && c == 6) ||
                        (r == 4 && c == 5))
                    {
                        gridArray[r, c] = 1;
                    }
                    else
                    {
                        gridArray[r, c] = 0;
                    }
                }
            }

        }

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
        public void Array2dtoArray1d(int[,] arr)
        {
            //convert 2d array to 1d array 
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
            {
                return true;
            }
            else
            {
                return false;
            }
        }





    }//end class
}