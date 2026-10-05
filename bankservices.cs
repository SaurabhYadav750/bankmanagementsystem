using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace bankmanagementsystem
{
    internal class bankservices
    {
        List<Account> accounts = new List<Account>();
        DatabaseConnection db = new DatabaseConnection();

        public void start()
        {
            SqlConnection connection = db.GetConnection();

            //connection.Open();

            Console.WriteLine("Connection successful!");

            //connection.Close();

            while (true) { 
                //Console.WriteLine("Welcome  to bank magement system ");
                Console.WriteLine("---------------------------------");
            Console.WriteLine("Bank management system");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("1. Create Account"); 
            Console.WriteLine("2.Deposit Money");
            Console.WriteLine("3.Withdraw Money");
            Console.WriteLine("4.Check Balance");
            Console.WriteLine("5.Account details");
            Console.WriteLine("6.Account listing");
            Console.WriteLine("7.Exit");

            Console.WriteLine("Enter your choice");
            string choice = Console.ReadLine();
            Console.WriteLine("You selected : "  +choice);

                switch (choice)
                {
                    case "1":
                        // Console.WriteLine("create account selected");
                        createaccount();
                        break;
                    case "2":
                        Console.WriteLine("Deposit money selected");
                        deposit();
                        break;
                    case "3":
                        Withdraw();
                        // Console.WriteLine("withdraw money selected");
                        break;
                    case "4":
                        BalanceInquiry();
                        //Console.WriteLine("check balance selected");
                        break;
                    case "5":
                        //Console.WriteLine("account details selected");
                        AccountDetails();
                        break;
                    case "6":
                        accountlisting();
                        //Console.WriteLine("account listing selected");
                        break;
                    case "7":
                        Exit();
                        return;
                        //Console.WriteLine("Exit selected");
                        break;
                    default:
                        Console.WriteLine("Invaild choice");
                        break;
                }
            }
        } // <-- added: close start()

        public void createaccount()
        {
            Account acc=new Account();
            Console.WriteLine("Enter account number");
            acc.accountnumber = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter account name");
            acc.accountname = Console.ReadLine();
            Console.WriteLine("Enter account balance");
            acc.accountbalance = Convert.ToDouble(Console.ReadLine());
            accounts.Add(acc);

           // Console.WriteLine("Account created successfully");

            SqlConnection connection = db.GetConnection();

            string query = @"insert into Accounts
                            ( accountnumber, accountname, accountbalance)
                            values
                            (@accountnumber, @accountname, @accountbalance)";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@accountnumber", acc.accountnumber);
            command.Parameters.AddWithValue("@accountname", acc.accountname);
            command.Parameters.AddWithValue("@accountbalance", acc.accountbalance);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            accounts.Add(acc);
            Console.WriteLine("Account created successfully");
        }

        public void deposit()

        {
            SqlConnection connection = null;
            try
            {
                Console.WriteLine("Enter account number");
                int accountnumber = Convert.ToInt32(Console.ReadLine());

                if (accountnumber <= 0)
                {
                    Console.WriteLine("Invalid account number.");
                    return;
                }

                Console.WriteLine("Enter amount to deposit");
                double amount = Convert.ToDouble(Console.ReadLine());
                if (amount <= 0)
                {
                    Console.WriteLine("invalid amount");
                    return;

                }

                 connection = db.GetConnection();

                string query = @"UPDATE  accounts set accountbalance=accountbalance+@amount
                           where accountnumber=@accountnumber";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@accountnumber", accountnumber);

                connection.Open();
                int row = command.ExecuteNonQuery();
                if (row > 0)
                {
                    Console.WriteLine("Deposit successful!");

                }
                else
                {
                    Console.WriteLine("Account not found.");
                }

                /* foreach (Account acc in accounts)
                {
                    if (acc.accountnumber == accountnumber)
                    {
                        acc.accountbalance = acc.accountbalance + amount;
                        Console.WriteLine("Deposit successful!");
                        Console.WriteLine("current balance :" + acc.accountbalance);
                        return;

                    }
                    Console.WriteLine("Account is not fund");
                }*/
               







            }
            catch(Exception ex)
            {
                Console.WriteLine("Invalid input. Please enter valid account number and amount "+ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        public void Withdraw()
        {
            Console.WriteLine("Enter your account number");
                int accountnumber = Convert.ToInt32(Console.ReadLine());
            if (accountnumber <= 0)
            {
                Console.WriteLine("invaild account nu8mber");
                return;
            }

            Console.WriteLine("Enter amount");
            double amount= Convert.ToDouble(Console.ReadLine());

            if (amount <= 0)
            {
                Console.WriteLine("invaild amont ");
                return;
            }

            SqlConnection connection = db.GetConnection();

            string query = @"select accountbalance from Accounts
                           where AccountNumber=@accountnumber";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Accountnumber", accountnumber);
            connection.Open();
            object result = command.ExecuteScalar();


            if (result == null)
            {
                Console.WriteLine("Account is not found");
                connection.Close();
                return;
            }
             double currentBalance = Convert.ToDouble(result);
            if (amount == currentBalance)
            {
                Console.WriteLine("Insufficient balance");
                connection.Close();
                return;
            }

            string updatequery = @"update accounts set accountbalance =accountbalance-@amount 
                                   where Accountnumber =@accountnumber";

            SqlCommand updateCommand = new SqlCommand(updatequery, connection);

            updateCommand.Parameters.AddWithValue("@amount", amount);
            updateCommand.Parameters.AddWithValue("@Accountnumber", accountnumber);


            int row = updateCommand.ExecuteNonQuery();

            if (row >0)
            {
                Console.WriteLine("withdraw successful!");
            }
            else
            {
                Console.WriteLine("withdraw is not successful");
            }

           /* foreach (Account acc in accounts)
            {
                if (acc.accountnumber == accountnumber)
                {
                    if (amount <= acc.accountbalance)
                    {
                        acc.accountbalance = acc.accountbalance - amount;
                        Console.WriteLine("withdraw is succesful");
                        Console.WriteLine("current balance :" + acc.accountbalance);


                    }
                    else
                    {
                        Console.WriteLine("Insufficient balance.");

                    }
                    return;
                }

            }
            Console.WriteLine("Account is not found");*/
      
        }
        public void BalanceInquiry()
        {
            Console.WriteLine("Enter account number");
            int accountnumber = Convert.ToInt32(Console.ReadLine());
            if(accountnumber<=0)
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            else
            {
                Console.WriteLine("Account number is valid.");
            }
            SqlConnection connection = db.GetConnection();
            string query= @"select accountbalance from Accounts
                           where AccountNumber=@accountnumber";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Accountnumber", accountnumber);

            connection.Open();
            object result = command.ExecuteScalar();
            if(result == null)
            {
                Console.WriteLine("Account not found.");
                connection.Close();
                return;
            }
            double balance = Convert.ToDouble(result);

            Console.WriteLine("Current Balance: " + balance);

            connection.Close();
            /*foreach (Account acc in accounts)
            {
               if(acc.accountnumber ==accountnumber)
                {
                    Console.WriteLine("Enter name :" + acc.accountname);
                    Console.WriteLine("current balance:" + acc.accountbalance);

                    return;
                }
            }
            Console.WriteLine("account not fund")*/
        }
           
        public void Exit()
        {
            Console.WriteLine("Program Exit;");
        }

         public void AccountDetails()
        {
            Console.WriteLine("Enter account number");
            int accountnumber = Convert.ToInt32(Console.ReadLine());

            if (accountnumber <= 0)
            {
                Console.WriteLine("invalid account number");
                return;
            }


            SqlConnection connection = db.GetConnection();

            string query = @"select Accountnumber,Accountname,Accountbalance
                            from Accounts where AccountNumber=@AccountNumber";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Accountnumber", accountnumber);
            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine("Account number" +reader["Accountnumber"]);
                Console.WriteLine("Accouont name" + reader["Accountname"]);
                Console.WriteLine("Account balnsce" + reader["Accountbalance"]);
            }

            else
            {
                Console.WriteLine("Account not found ");
            }
            reader.Close();
            connection.Close();
        

           /* foreach (Account acc in accounts)
            {
                if (acc.accountnumber == accountnumber)
                {
                    Console.WriteLine("Account Number: " + acc.accountnumber);
                    Console.WriteLine("Account Name: " + acc.accountname);
                    Console.WriteLine("Account Balance: " + acc.accountbalance);
                    return;
                }
            }
            Console.WriteLine("Account not found.");*/
        }
        public void accountlisting()
        {
            /*if (accounts.Count==0)
            {
                Console.WriteLine("No accounts found.");
                return;
            }*/

            SqlConnection connection = db.GetConnection();

            string query = @"select accountnumber, accountname, accountbalance from accounts ";
            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("Account Number: " + reader["Accountnumber"]);
                Console.WriteLine("Account Name: " + reader["Accountname"]);
                Console.WriteLine("Account Balance: " + reader["Accountbalance"]);
                Console.WriteLine("-------------------------");
            }

            reader.Close();
            connection.Close();
        


        /*
        foreach(Account acc in accounts)
        {
            Console.WriteLine("------------------------");
            Console.WriteLine("Account Number: " + acc.accountnumber);
            Console.WriteLine("Account Name: " + acc.accountname);
            Console.WriteLine("Account Balance: " + acc.accountbalance);
        }*/
    }

    }

}
