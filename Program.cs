using System.Text;

namespace LockOptimistingWritingFiles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            File.Delete("file.txt");

            int interactions = 5;
            Thread[] threads = new Thread[interactions];
            for (int i = 0; i < threads.Length; i++)
            {
                var t = new Thread(() =>
                {
                    FileWriter fileWriter = new FileWriter();
                    fileWriter.WriteVersionOne();
                    fileWriter.WriteVersionTwo();
                });
                t.Start();
            }

            Console.ReadKey();
        }
    }

    internal class FileWriter
    {
        /* 
         * I create this object as static because when WriteVersionTwo is called in the same thread
         * this object already exists and doesn't allow to start writing unless WriteVersionOne finish
        */
        private static object locker = new object();

        public void WriteVersionOne()
        {
            Console.WriteLine("One");
            lock (locker)
            {
                StreamWriter sw = new StreamWriter("file.txt", true);
                for (int i = 0; i < 5; i++)
                {
                    sw.WriteLine($"One - {i + 1}");
                }
                sw.Close();
            }
        }

        public void WriteVersionTwo()
        {
            Console.WriteLine("Two");
            lock (locker)
            {
                StreamWriter sw = new StreamWriter("file.txt", true);
                for (int i = 0; i < 5; i++)
                {
                    sw.WriteLine($"Two - {i + 1}");
                }
                sw.Close();
            }
        }
    }
}