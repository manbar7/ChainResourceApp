// See https://aka.ms/new-console-template for more information
using ChainResourceApp;
using Microsoft.VisualBasic;

public class Program    
{
   

    public static void Main(string[] args)
    {     

        ExchangeRateList exchangeRateList = new ExchangeRateList();

        var taskData = exchangeRateList.GetValue();
        taskData.Wait();

        Console.ReadLine();
    }



}
