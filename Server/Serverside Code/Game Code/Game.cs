using PlayerIO.GameLibrary;
using System;

namespace Chess.Server {
    public class Player : BasePlayer {
    }

    [RoomType("ChessRoom")]
    public class GameCode : Game<Player> {

        // This method is called when an instance of your the game is created
        public override void GameStarted() =>
            // anything you write to the Console will show up in the 
            // output window of the development server
            Console.WriteLine("Game is started: " + RoomId);

        // This method is called when the last player leaves the room, and it's closed down.
        public override void GameClosed() => Console.WriteLine("RoomId: " + RoomId);

        // This method is called whenever a player joins the game
        public override void UserJoined(Player player) {
            foreach(Player pl in Players) {
                if(pl.ConnectUserId != player.ConnectUserId) {
                    pl.Send("PlayerJoined", player.ConnectUserId);
                    player.Send("PlayerJoined", pl.ConnectUserId);
                }
            }
        }

        // This method is called when a player leaves the game
        public override void UserLeft(Player player) => Broadcast("PlayerLeft", player.ConnectUserId);

        // This method is called when a player sends a message into the server code
        public override void GotMessage(Player player, Message m) {
            switch(m.Type) {
                // called when a player clicks on the ground
                case nameof(MessageType.Move):
                    Broadcast("Move", m.GetInt(0), m.GetInt(1), m.GetInt(2), m.GetInt(3));
                    Console.WriteLine($"Move {Convert.ToChar('a' + m.GetInt(0))}{m.GetInt(1)} to {Convert.ToChar('a' + m.GetInt(2))}{m.GetInt(3)}");
                    break;
            }
        }
    }
}