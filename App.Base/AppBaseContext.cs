using PoorFff;
using PoorFff.ProdCons;
using PoorFff.ProdCons.RabbitMQ;
using PoorFff.PubSub;
using PoorFff.PubSub.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Base
{
    public class AppBaseContext: IPubSub, IProdCons
    {
        private static readonly Lazy<AppBaseContext> _instance = new Lazy<AppBaseContext>(() => new AppBaseContext());
        public static AppBaseContext Instance => _instance.Value;
        private AppBaseContext() { }

        private string RabbitMQConnectionString { get; set; }
        public IPubSub Sub => GetPubSub();
        public IProdCons Cons => GetProdCons();

        public string Scope { get; private set; }
        public void Initialize(IPfConfiguration configuration)
        {
            RabbitMQConnectionString = configuration.Get<string>("AppBaseContext.RabbitMQConnectionString");
            Scope = configuration.Get<string>("Scope", ()=>Guid.NewGuid().ToString("N"));
        }

        private IPubSub GetPubSub()
        {
            return new PubSubRabbitProvider(RabbitMQConnectionString);
        }

        private IProdCons GetProdCons()
        {
            return new ProdConsRabbitProvider(RabbitMQConnectionString);
        }

        public void Produce<T>(string queueName, T msg)
        {
            Cons.Produce($"{Scope}.{queueName}", msg);
        }

        public void Produce<T>(string queueName, T msg, TimeSpan delayTime)
        {
            delayTime = delayTime.Subtract(new TimeSpan(0, 0, 0, 0, delayTime.Milliseconds));
            Cons.Produce($"{Scope}.{queueName}", msg, delayTime);
        }

        public void Consume<T>(string queueName, Action<string, T> callback)
        {
            Cons.Consume($"{Scope}.{queueName}", callback);
        }

        public long Publish<T>(string channel, T msg)
        {
            return Sub.Publish($"{Scope}.{channel}", msg);
        }

        public void Subscribe<T>(string channel, Action<string, T> callback)
        {
            Sub.Subscribe($"{Scope}.{channel}", callback);
        }

        public void Unsubscribe(string channelOrPattern)
        {
            Sub.Unsubscribe($"{Scope}.{channelOrPattern}");
        }

        public void UnsubscribeAll()
        {
            Sub.UnsubscribeAll();
        }
    }
}
