using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SignalRChat
{
    public class slotHub : Hub
    {
        public void Genslots()
        {
            Clients.Caller.numberreturn(slotgen(1)+ slotgen(2)+ slotgen(3));
        }

        public void Getledger()
        {
            try
            {

                string constr = ConfigurationManager.ConnectionStrings["AppDataDB"].ToString();
                using (SqlConnection Con = new SqlConnection(constr))
                {
                    Con.Open();
                    SqlCommand command = new SqlCommand(@"EXEC [dbo].[PullLedger]; ", Con);

                    SqlDataReader cmdread = command.ExecuteReader();
                    cmdread.Read();

                    if (cmdread.HasRows)
                    {
                        Clients.Caller.returnledger(cmdread[0].ToString());
                    }

                }

            }
            catch (Exception errortxt)
            {
                Clients.Caller.errorrtn("ERROR: " + errortxt.Message);
            }
            
        }

        public void Genrewards(string member)
        {
            try
            {

                string constr = ConfigurationManager.ConnectionStrings["AppDataDB"].ToString();
                using (SqlConnection Con = new SqlConnection(constr))
                {
                    Con.Open();
                    SqlCommand command = new SqlCommand(@"EXEC [dbo].[checkrewards]@member = @member; ", Con);
                    command.Parameters.AddWithValue("member", member);

                    SqlDataReader cmdread = command.ExecuteReader();
                    cmdread.Read();

                    if(cmdread.HasRows)
                    {
                        Clients.Caller.rewardsreturn(cmdread[0].ToString());
                    }

                }

            }
            catch (Exception errortxt)
            {
                Clients.Caller.errorrtn("ERROR: " + errortxt.Message);
            }

           
        }

        public void Slotresult(string number, string member)
        {
            try
            {
                bool win = false;
                int wincredits = 0;

                switch (number)
                {
                    case "777":
                        win = true;
                        wincredits = 5;
                        Clients.Others.annoucewinner(member);
                        break;

                    case "333":
                        win = true;
                        wincredits = 2;
                        Clients.Others.annoucewinner(member);
                        break;

                    default:

                        if (number == "0" + number.Remove(0, 1))
                        {
                            win = true;
                            Clients.Others.annoucewinner(member);
                            wincredits = 5;
                        }
                        else
                        {
                            win = false;
                            wincredits = 0;
                        }


                        break;
                }

                trackslotplay(member, win, wincredits, 1);
                Clients.Caller.resultreturn(number, win, wincredits);
            }
            catch (Exception errortxt)
            {
                Clients.Caller.errorrtn("ERROR: "+errortxt.Message);
            }
}

        private void trackslotplay(string member, bool win, int wincredits, int reward)
        {
            try
            {

                string constr = ConfigurationManager.ConnectionStrings["AppDataDB"].ToString();
                using (SqlConnection Con = new SqlConnection(constr))
                {
                    Con.Open();
                    SqlCommand command = new SqlCommand(@"EXEC [dbo].[trackplay]@member = @member,
                    @credit = 1,
                    @reward = @reward,
                    @winamt = @wincredits; ", Con);
                    command.Parameters.AddWithValue("member",member);
                    command.Parameters.AddWithValue("wincredits", wincredits);
                    command.Parameters.AddWithValue("reward", reward);

                    command.ExecuteNonQuery();

                }

            }
            catch (Exception errortxt)
            {
                Clients.Caller.errorrtn("ERROR: "+errortxt.Message);
            }

        }

        private static string slotgen(int slot) 
        {
            string randomseed = new Random().Next(100, 1000).ToString();
            return randomseed.Substring(slot);
        }
        


    }
}
