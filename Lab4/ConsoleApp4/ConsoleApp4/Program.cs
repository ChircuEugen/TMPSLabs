using System;
using System.Collections.Generic;
using System.Globalization;

namespace ConsoleApp4
{
    class Program
    {
        static void Main(string[] args)
        {
            // bruh
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            CardOwner Julia = new CardOwner("Julia", "Owell", 1233.29f, 1234);
            ATMMachine bankomat = new ATMMachine(Julia);

            Console.WriteLine("ATM Machine Current state : " + bankomat.ATMMachineState.GetType().Name);

            // INSERTING THE CARD
            bankomat.InsertDebitCard();
            Console.WriteLine(" ");
            Console.WriteLine("ATM Machine Current state : " + bankomat.ATMMachineState.GetType().Name);
            Console.WriteLine(" ");

            // operation with INSERTED CARD
            bankomat.EnterPin();
            bankomat.WithdrawMoney();

            // EJECTING THE CARD
            bankomat.EjectCard();
            Console.WriteLine(" ");
            Console.WriteLine("ATM Machine Current state : " + bankomat.ATMMachineState.GetType().Name);

            // operation with EJECTED card
            bankomat.EnterPin();
            bankomat.WithdrawMoney();
        }
    }

    public class CardOwner
    {
        string firstName;
        string lastName;
        public double moneyAmount;
        public int pin;

        public CardOwner(string firstName, string lastName, double moneyAmount, int pin)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.moneyAmount = moneyAmount;
            this.pin = pin;
        }


    }

    public interface IATMState
    {
        void InsertDebitCard(); // ???
        void EjectDebitCard();
        void EnterPin(); // ???
        void WithdrawMoney(); // ???
    }

    public class NotInsertedCardState : IATMState
    {
        private readonly CardOwner owner;

        public NotInsertedCardState(CardOwner owner) { this.owner = owner; }
        public void InsertDebitCard() //
        {
            Console.WriteLine("DebitCard Inserted");
        }
        public void EjectDebitCard()
        {
            Console.WriteLine("You cannot eject the Debit CardNo, as no Debit Card in ATM Machine slot");
        }
        public void EnterPin() //
        {
            Console.WriteLine("You cannot enter the pin, as No Debit Card in ATM Machine slot");
           // return false;
        }
        public void WithdrawMoney() //
        {
            Console.WriteLine("You cannot withdraw money, as No Debit Card in ATM Machine slot");
        }
    }

    public class InsertedCardState : IATMState
    {
        private readonly CardOwner owner;

        public InsertedCardState(CardOwner owner) { this.owner = owner; }
        public void InsertDebitCard()
        {
            Console.WriteLine("You cannot insert the Debit Card, as the Debit card is already there ");
        }
        public void EjectDebitCard()
        {
            Console.WriteLine("Debit Card is ejected");
        }
        public void EnterPin()
        {
           // bool allGood = false;
            Console.Write("PIN CODE: ");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option == owner.pin)
            {
                Console.WriteLine("Pin number has been entered correctly");
                //allGood = true;
            }
            else
            { 
                Console.WriteLine("Wrong PIN. Try again."); 
                EnterPin(); 
            }
           // return allGood;
        }
        public void WithdrawMoney()
        {
            Console.Write("Withdraw: ");
            double option = Convert.ToDouble(Console.ReadLine());
            owner.moneyAmount -= option;
            Console.WriteLine("Money has been withdrawn");
            Console.WriteLine($"SOLD: {owner.moneyAmount}");
        }
    }

    public class ATMMachine //: IATMState
    {
        CardOwner owner;
        public IATMState ATMMachineState = null;

        public ATMMachine(CardOwner owner)
        {
            this.owner = owner;
            // Default state in creating: NotInserted
            ATMMachineState = new NotInsertedCardState(owner);
        }

        public void InsertDebitCard()
        {
            Console.WriteLine("Credit card was inserted");
            if (ATMMachineState is NotInsertedCardState) ATMMachineState = new InsertedCardState(owner);
        }

        public void EnterPin()
        {
            ATMMachineState.EnterPin();
            //if(ATMMachineState.EnterPin())
            
               // if (ATMMachineState is NotInsertedCardState) ATMMachineState = new InsertedCardState(owner);
            

        }

        public void EjectCard()
        {

            ATMMachineState.EjectDebitCard();

            if (ATMMachineState is InsertedCardState) ATMMachineState = new NotInsertedCardState(owner);
        }

        public void WithdrawMoney()
        {
            ATMMachineState.WithdrawMoney();
        }
    }
}
