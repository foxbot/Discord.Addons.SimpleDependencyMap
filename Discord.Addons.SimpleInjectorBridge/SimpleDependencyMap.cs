using System;
using Discord.Commands;
using SimpleInjector;

namespace Discord.Addons.SimpleInjectorBridge
{
    public class SimpleDependencyMap : IDependencyMap
    {
        public static SimpleDependencyMap Empty => new SimpleDependencyMap();

        public Container Container { get; }

        public SimpleDependencyMap()
        {
            Container = new Container();
        }

        public SimpleDependencyMap(Container container)
        {
            Container = container;
        }

        public void Add<T>(T obj) where T : class
            => AddFactory<T>(() => obj);

        public bool TryAdd<T>(T obj) where T : class
            => TryAddFactory<T>(() => obj);

        public void AddTransient<T>() where T : class, new()
            => Container.Register<T>();

        public bool TryAddTransient<T>() where T : class, new()
            => TryAddTransient<T, T>();

        public void AddTransient<TKey, TImpl>() where TKey : class where TImpl : class, TKey, new()
            => Container.Register<TKey, TImpl>();

        public bool TryAddTransient<TKey, TImpl>() where TKey : class where TImpl : class, TKey, new()
        {
            try
            {
                Container.Register<TKey, TImpl>();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void AddFactory<T>(Func<T> factory) where T : class
            => Container.Register<T>(factory);

        public bool TryAddFactory<T>(Func<T> factory) where T : class
        {
            try
            {
                Container.Register<T>(factory);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public T Get<T>() where T : class
            => Container.GetInstance<T>();

        public bool TryGet<T>(out T result) where T : class
        {
            try
            {
                result = Container.GetInstance<T>();
                return true;
            }
            catch (Exception e)
            {
                result = default(T);
                return false;
            }
        }

        public object Get(Type t)
        {
            return Container.GetInstance(t);
        }

        public bool TryGet(Type t, out object result)
        {
            try
            {
                result = Container.GetInstance(t);
                return true;
            }
            catch (Exception e)
            {
                result = null;
                return false;
            }
        }
    }
}