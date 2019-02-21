using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            Blockchain Mafcoin = new Blockchain();
            Mafcoin.AddBlock(new Block(DateTime.Now, null, "{sender: Maf, receiver: dezeman, amount: 10}"));
            Mafcoin.AddBlock(new Block(DateTime.Now, null, "{sender: deze, receiver: hoe, amount: 10}"));
            Mafcoin.AddBlock(new Block(DateTime.Now, null, "{sender: Maf, receiver: waar, amount: 10}"));

            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            //Console.WriteLine("Hello World!");
            //Console.ReadKey();
            Console.WriteLine(JsonConvert.SerializeObject(Mafcoin, Formatting.Indented));
            Console.ReadKey();
            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }


    public class Block
    {
        public int Index { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }
        public string Data { get; set; }

        public Block(DateTime timeStamp, string previousHash, string data)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = previousHash;
            Data = data;
            Hash = CalculateHash();
        }
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();

            byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{Data}");
            byte[] outputBytes = sha256.ComputeHash(inputBytes);

            return Convert.ToBase64String(outputBytes);
        }
    }


    public class Blockchain
    {
        public IList<Block> Chain { set; get; }

        public Blockchain()
        {
            InitializeChain();
            AddGenesisBlock();
        }
        public void InitializeChain()
        {
            Chain = new List<Block>();
        }
        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, "{}");
        }
        public void AddGenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public Block GetLatestBlock()
        {
            return Chain[Chain.Count - 1];
        }
        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.Index = latestBlock.Index + 1;
            block.PreviousHash = latestBlock.Hash;
            block.Hash = block.CalculateHash();
            Chain.Add(block);
        }

        public bool IsValid()
        {
            for (int i = 1; i < Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i - 1];

                if (currentBlock.Hash != currentBlock.CalculateHash())
                {
                    return false;
                }

                if (currentBlock.PreviousHash != previousBlock.Hash)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
