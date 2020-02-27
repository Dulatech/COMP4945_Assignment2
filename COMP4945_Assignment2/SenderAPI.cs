using System;
using System.Threading;
using COMP4945_Assignment2;

namespace NetworkComm
{
    /*
     * For every msg sent, the first line is the HEADER.
     * 
     * For game state msgs, the second line is the Guid of the game, and third line is in the form of
     * [type],[msg]
     * where type is 0 for movement, 1 for bullet fired, 2 for bullet collides with vehicle etc.
     * and msg is the values that need to be sent for the specific type of msg
     * for example for type 0 (movement), the message format is
     * Guid:id , int:playerNum , int:X , int:Y , int:direction   (without the spaces)
     * 
     * For game connection messages, (invite, join req, join resp, etc)
     * the second line is the type, and third line is the msg.
     * The format of second and third line for each type of msg is listed below
     * 
     * INVITAION:
     * 0
     * Guid:gameID , int:playerNum(the number for next player, 0 and 2 is for tank, 1 and 3 is for plane)
     * 
     * REQUEST TO JOIN:
     * 1
     * Guid:gameID , Guid:reqID(ID of requesting player) , int:playerNum(same number from the invitation msg)
     * 
     * ACCEPT JOIN REQ:
     * 2
     * Guid:gameID , Guid:reqID(ID of requesting player)
     * 
     * DECLINE JOIN REQ:
     * 3
     * Guid:gameID , Guid:reqID(ID of requesting player)
     */
    public class SenderAPI
    {
        // just in case other applications use this port for multicast
        public static readonly string HEADER = "SOMETHING UNIQUE";

        private static void Send(int msgType, string msg)
        {
            if (msgType == -1) // game msg
            {
                msg = HEADER + "\n" + GameArea.gameID + "\n" + msg;
            }
            else
            {
                msg = HEADER + "\n" + msgType + "\n" + msg;
            }
            /***********************************************************/
            MulticastSender.SendMsg(msg);
            /***********************************************************/
            if (msgType != 0)
                PrintSentMsgs(msg);
        }
        // info on gameMsgTypes can be found in MulticastReceiver.HandleGameMsg()
        public static void SendGameMsg(int gameMsgType, string msg)
        {
            Send(-1, gameMsgType + "," + NetworkController.ID + "," + GameArea.playerNum + "," + msg);
        }
        // multicast invitations every half a second
        // this method should be passed on to a new background thread only if the user is a host of a new game
        // i.e. playerNum == 0
        public static void SendInvitations()
        {
            while (true)
            {
                if (GameArea.currentNumOfPlayers < GameArea.MAX_PLAYERS)
                {
                    Send(0, GameArea.gameID + "," + GameArea.nextPlayer);
                }
                Thread.Sleep(500);
            }
        }
        public static void SendJoinReq(Guid gameID, Guid reqID, int playerNum)
        {
            Send(1, gameID + "," + reqID + "," + playerNum);
        }
        public static void SendJoinResp(string reqId, bool accepted)
        {
            int type = accepted ? 2 : 3;
            Send(type, GameArea.gameID + "," + reqId);
        }
        private static void PrintSentMsgs(string msg)
        {
            System.Diagnostics.Debug.WriteLine("\n\tSent:");
            msg = msg.Replace("\n", "\n\t");
            System.Diagnostics.Debug.WriteLine("\t" + msg);
        }
    }
}
