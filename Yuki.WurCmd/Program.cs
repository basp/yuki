namespace Yuki.WurCmd
{
    using PowerArgs;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    class WurCmdProgram
    {
        static readonly ConnectionFactory Factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        [HelpHook]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Generate a PDF report")]
        public void Generate(GenerateArgs args)
        {
            var result = Utils.CreatePdf(args.Source);
            Console.WriteLine(result.Item2);
        }

        [ArgActionMethod, ArgDescription("Ping the daemon")]
        public void Ping()
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = "Hello World!";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        [ArgActionMethod, ArgDescription("Start in deamon mode")]
        public void Start()
        {
            using (var connection = Factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "hello",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);

                    channel.BasicAck(
                        deliveryTag: ea.DeliveryTag, 
                        multiple: false);
                };

                channel.BasicConsume(
                    queue: "hello",
                    noAck: false,
                    consumer: consumer);

                Console.ReadLine();
            }
        }
    }

    class GenerateArgs
    {
        [ArgRequired, ArgDescription("Path to the XML source file"), ArgPosition(1)]
        public string Source { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Args.InvokeAction<WurCmdProgram>(args);
        }
    }
}
