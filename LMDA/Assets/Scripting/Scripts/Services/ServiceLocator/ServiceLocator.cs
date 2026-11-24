using System;
using System.Collections.Generic;

public class ServiceLocator : IServiceLocator
{
    private static Dictionary<Type, object> _services = new();

    public void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public void Register(Type type, object service)
    {
        _services[type] = service;
    }

    public bool Unregister<T>()
    {
        return Unregister(typeof(T));
    }

    public bool Unregister(Type type)
    {
        return _services.Remove(type);
    }

    public T Get<T>()
    {
        return (T)_services[typeof(T)];
    }

    public object Get(Type type)
    {
        return _services[type];
    }

    public T TryGet<T>()
    {
        _services.TryGetValue(typeof(T), out var service);
        return (T)service;
    }

    public object TryGet(Type type)
    {
        _services.TryGetValue(type, out var service);
        return service;
    }

    public bool IsRegistered<T>()
    {
        return IsRegistered(typeof(T));
    }

    public bool IsRegistered(Type type)
    {
        return _services.ContainsKey(type);
    }
}