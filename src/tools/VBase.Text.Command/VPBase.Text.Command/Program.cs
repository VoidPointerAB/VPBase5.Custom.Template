namespace VPBase.Text.Command
{
    class Program
    {
        static void Main(params string[] args)
        {
            var manager = ProcessArgumentManagerFactory.Create();

            manager.Execute(args);
        }        
    }
}
