// Feel free to use/modify this code as a start to pa1

using System;
using static System.Console;

namespace Bme121
{
    static class Program
    {
        static int[ , ] board = new int[ 6, 8 ]; 
        //0 - open tile
        //1 - removed tile
        //2 - P1 pawn
        //3 - P2 pawn
        static int[] pawn1, pawn2;
        static string[] letters;
        
        static void Main( )
        {
            bool gameRunning = true, turnPlayer = true, canMove;
            pawn1 = new int[]{0, 0};
            pawn2 = new int[]{5, 7};
            board[0, 0] = 2;
            board[5, 7] = 3;
            letters = new string[]{ "a","b","c","d","e","f","g","h","i","j","k","l",
                "m","n","o","p","q","r","s","t","u","v","w","x","y","z"};
                
            //Explain how to input
            Console.Clear();
            WriteLine("Input coordinates in the form xy");
            WriteLine("Where x is the row, and y is the column");
            WriteLine("For example, ab denotes row a column b");
            WriteLine();
            WriteLine("\u2590\u2588\u258c denotes an open tile");
            WriteLine(" \u25a0  denotes a removed tile");
            
            WriteLine("Press enter to continue");
            ReadLine();
            
            while(gameRunning)
            {
				Console.Clear();
				DrawGameBoard();
				if (turnPlayer)
					WriteLine("It is Player A's turn");
				else
					WriteLine("It is Player B's turn");
				
				canMove = MovePawn(turnPlayer);
				if (!canMove)
				{
					WriteLine("No moves: Game Over!");
					Write("\u250c");
					for (int i = 0; i < 13; i++)
						Write("\u2500");
					WriteLine("\u2510");
					if (turnPlayer)
						WriteLine("\u2502Player B wins\u2502");
					else
						WriteLine("\u2502Player A wins\u2502");
					Write("\u2514");
					for (int i = 0; i < 13; i++)
						Write("\u2500");
					WriteLine("\u2518");
					gameRunning = false;
					continue;
				}
				
				Console.Clear();
				DrawGameBoard();
				RemoveTile(turnPlayer);
				
				Console.Clear();
				DrawGameBoard();
				
				turnPlayer = !turnPlayer;
            }
            WriteLine();
            WriteLine("Thanks for Playing!");
        }
        
        static bool MovePawn(bool player)
        {
			int[] currentPawn;
			string[] validMoves = new string[8];
			string pawnMove = "";
			bool temp = true, noMoves = true;
			
			if (player)
				currentPawn = (int[])pawn1.Clone();
			else
				currentPawn = (int[])pawn2.Clone();
			
			WriteLine("Select a move for your pawn:");
			if (currentPawn[0] > 0 && currentPawn[1] > 0)//up and left
				if (IsEmpty(currentPawn[0] - 1, currentPawn[1] - 1))
					validMoves[0] = letters[currentPawn[0] - 1] + letters[currentPawn[1] - 1];
			if (currentPawn[0] > 0)//up
				if (IsEmpty(currentPawn[0] - 1, currentPawn[1]))
					validMoves[1] = letters[currentPawn[0] - 1] + letters[currentPawn[1]];
			if (currentPawn[0] > 0 && currentPawn[1] < 7)//up and right
				if (IsEmpty(currentPawn[0] - 1, currentPawn[1] + 1))
					validMoves[2] = letters[currentPawn[0] - 1] + letters[currentPawn[1] + 1];
			if (currentPawn[1] > 0)//left
				if (IsEmpty(currentPawn[0], currentPawn[1] - 1))
					validMoves[3] = letters[currentPawn[0]] + letters[currentPawn[1] - 1];
			if (currentPawn[1] < 7)//right
				if (IsEmpty(currentPawn[0], currentPawn[1] + 1))
					validMoves[4] = letters[currentPawn[0]] + letters[currentPawn[1] + 1];
			if (currentPawn[0] < 5 && currentPawn[1] > 0)//down and left
				if (IsEmpty(currentPawn[0] + 1, currentPawn[1] - 1))
					validMoves[5] = letters[currentPawn[0] + 1] + letters[currentPawn[1] - 1];
			if (currentPawn[0] < 5)//down
				if (IsEmpty(currentPawn[0] + 1, currentPawn[1]))
					validMoves[6] = letters[currentPawn[0] + 1] + letters[currentPawn[1]];
			if (currentPawn[0] < 5 && currentPawn[1] < 7)//down and right
				if (IsEmpty(currentPawn[0] + 1, currentPawn[1] + 1))
					validMoves[7] = letters[currentPawn[0] + 1] + letters[currentPawn[1] + 1];
					
			foreach (string move in validMoves)
			{
				if (move != null)
				{
					WriteLine(move);
					noMoves = false;
				}
			}
			WriteLine("---");
			if (noMoves)
			{
				return false;
			}
			
			while(temp)
			{
				pawnMove = ReadLine();
				if (pawnMove == "exit")
					Environment.Exit(0);
				if (Array.IndexOf(validMoves, pawnMove) > -1)
					temp = false;
				else
					WriteLine("Invalid Move");
			}
			
			currentPawn[0] = (int)pawnMove[0] - 97;
			currentPawn[1] = (int)pawnMove[1] - 97;
			UpdateBoard(currentPawn[0], currentPawn[1], true, player);
			
			if (player)
				pawn1 = (int[])currentPawn.Clone();
			else
				pawn2 = (int[])currentPawn.Clone();
				
			return true;
		}
        
        static void RemoveTile(bool player)
        {
			string tile;
			bool temp = true;
			int tilex = -1, tiley = -1;
			
			WriteLine("Choose a tile to remove");
			while (temp)
			{
				tile = ReadLine();
				if (tile == "exit")
					Environment.Exit(0);
				tilex = (int)tile[0] - 97;
				tiley = (int)tile[1] - 97;
				temp = !IsEmpty(tilex, tiley);
				if (tilex == 0 && tiley == 0 || tilex == 5 && tiley == 7)
					temp = false;
				if (temp)
					WriteLine("Invalid tile choice");
			}
			
			UpdateBoard(tilex, tiley, false, player);
        }
        
        static bool IsEmpty(int row, int col)
        {
			if (row < 0 || row > 5 || col < 0 || col > 7)
				return false;
			if (board[row, col] == 0)
				return true;
			else
				return false;
        }
        
        static void UpdateBoard(int row, int col, bool isPawn, bool player)
        {
			//check if valid
			if (row < 0 || row > 5 || col < 0 || col > 7)
				return;
			if (board[row, col] > 0)
				return;
			
			//move pawn
			if (isPawn)
			{
				if (player)
				{
					board[row, col] = 2;
					board[pawn1[0], pawn1[1]] = 0;
				}
				else
				{
					board[row, col] = 3;
					board[pawn2[0], pawn2[1]] = 0;
				}
			}
			//remove tile
			else
				board[row, col] = 1;
        }
        
        static void DrawGameBoard( )
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            //const string sp = " ";      // space
            const string pa = "A";      // pawn A
            const string pb = "B";      // pawn B
            const string bb = "\u25a0"; // block
            const string fb = "\u2588"; // full block
            const string lh = "\u258c"; // left half block
            const string rh = "\u2590"; // right half block
                
            // Draw the first row and top board boundary.
            Write("     ");
            for (int i = 0; i < 8; i++)
				Write(letters[i] + "   ");
			WriteLine();
            Write( "   " );
            for( int c = 0; c < board.GetLength( 1 ); c ++ )
            {
                if( c == 0 ) Write( tl );
                Write( "{0}{0}{0}", h );
                if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", tr ); 
                else                                Write( "{0}", hb );
            }
            WriteLine( );
            
            // Draw the board rows.
            for( int r = 0; r < board.GetLength( 0 ); r ++ )
            {
                Write( " {0} ", letters[ r ] );
                
                // Draw the row contents.
                for( int c = 0; c < board.GetLength( 1 ); c ++ )
                {
                    if( c == 0 ) Write( v );
                    if( board[ r, c ] == 0) Write( "{0}{1}", rh + fb + lh, v );
                    else if (board[r, c] == 1) Write("{0}{1}", " " + bb + " ", v);
                    else if (board[r, c] == 2) Write("{0}{1}", " " + pa + " ", v);
                    else if (board[r, c] == 3) Write("{0}{1}", " " + pb + " ", v);
                    else                Write( "{0}{1}", "   ", v );
                }
                WriteLine( );
                
                // Draw the boundary after the row.
                if( r != board.GetLength( 0 ) - 1 )
                { 
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( vr );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", vl ); 
                        else                                Write( "{0}", hv );
                    }
                    WriteLine( );
                }
                else
                {
                    Write( "   " );
                    for( int c = 0; c < board.GetLength( 1 ); c ++ )
                    {
                        if( c == 0 ) Write( bl );
                        Write( "{0}{0}{0}", h );
                        if( c == board.GetLength( 1 ) - 1 ) Write( "{0}", br ); 
                        else                                Write( "{0}", ha );
                    }
                    WriteLine( );
                }
            }
        }
    }
}
