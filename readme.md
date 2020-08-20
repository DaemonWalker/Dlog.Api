# Daemon的博客后台

## 技术栈
1. 后台使用 .Net Core WebApi (.Net Core 版本3.1以上,.Net 5.0以上)
2. 缓存使用Redis，重写ICache接口并在[Startup.cs](https://github.com/DaemonWalker/Dlog.Api/blob/master/Dlog.Api/Startup.cs#L52)的IOC配置总进行替换
3. 数据库使用MongoDB，重写IDatabase接口并在[Startup.cs](https://github.com/DaemonWalker/Dlog.Api/blob/master/Dlog.Api/Startup.cs#L53)的IOC配置中进行替换

## 使用方法
克隆仓库，并将博文按照下面的结构放到[Blogs](https://github.com/DaemonWalker/Dlog.Api/tree/master/Dlog.Api/Blogs)文件夹中

## 博客文件夹结构
1. 新建一个文件夹，名称请用英文(中文的没测试好不好使)，这个用于url显示 
2. 把下列文件放入第一步新建的文件夹中
3. content.md为文章内容，请用markdown语法编写
4. info.md为文章的各种信息
    ```json
    {
        "date": "2020-07-26",
        "summary": "简单讲讲博客怎么搭起来的，累死我了...",
        "title": "搭建博客的流水账",
        "cover": "avatar.png",
        "tags": ["Docker"]
    }
    ```
# 版本说明
## Release-1.0 ->1.0.1
+ 博客后端上线啦🍾🍾🍾
+ 使用比较复杂的博客信息系统，就是一大堆文件，各存各的信息，用起来比较费劲-_-||
+ 使用Hangfire定时任务扫描博客变动
+ 负责静态文件也就是图片的分发
## Release-1.2 ->1.2.0
+ 版本号主要是因为前端刷到了1.2所以后端也跟着1.2了~
+ 使用info.json代替原来那一大堆文件
+ 博客缩略图不在存放在各文件夹内而是集体管理，并由前端负责分发
+ 取消hangfire定时任务，改用调用api的方式进行更新，尽量轻量化博客后端