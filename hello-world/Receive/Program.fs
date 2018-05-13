open System
open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text

[<EntryPoint>]
let main argv =
    let factory = new ConnectionFactory()
    factory.HostName <- "localhost"
    use connection = factory.CreateConnection()
    use channel = connection.CreateModel()
    channel.QueueDeclare("hello",false, false, false, null) |> ignore
    
    let consumer = new EventingBasicConsumer(channel)

    let printReceivedMessage model (event:BasicDeliverEventArgs) =
        let body = event.Body
        let message = Encoding.UTF8.GetString body
        Console.WriteLine("Received message: {0}", message)
         
    let receiveMessageEventHandler = new EventHandler<BasicDeliverEventArgs>(printReceivedMessage)
    consumer.Received.AddHandler(receiveMessageEventHandler)
    channel.BasicConsume("hello",true,consumer) |> ignore
    Console.WriteLine "Press any key to continue"
    Console.ReadKey() |> ignore
    0