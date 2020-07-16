# IOC容器
## IOC容器的作用
通俗的讲，IOC容器就是一个映射关系以及对生成对象进行生命周期管理的框架。

## 映射关系
通常上映射关系是一对一的，一个接口/抽象类只有一个实现。在一些特殊的情况，比如log工具类可能会有一对多的关系。

## 生成对象
我们在代码中请求的对象会保管在IOC容器中，并根据他们的生命周期类型在特定的时候释放他们

## 生命周期
比较通常的生命周期有：
+ 全局单例，就是只有一个对象 
+ 作用域内单例，就是一个作用范围内单例。通常是一个Http请求，正常的用法如下，因为所在作用域不同，所以foo1和foo2不是同一个对象。
```
IServiceCollection services = new ServiceCollection();
services.AddScoped<IFoo, Foo>();
var serviceProvider = services.BuildServiceProvider();
var foo1 = serviceProvider.GetService<IFoo>();
using (var scope = serviceProvider.CreateScope())
{
    var foo2 = serviceProvider.GetService<IFoo>();
    Debug.Assert(foo1 != foo2);
}
```
+ 每次请求新实例，就是每次请求的时候都会生成一个新的对象。当初写SpringCloud的时候，好像这个模式每次对于对象的调用都会生成一个新的对象，很神奇-_-||



