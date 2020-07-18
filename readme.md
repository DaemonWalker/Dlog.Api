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
3. summary.png为首页显示的缩略图
4. content.md为文章内容，请用markdown语法编写
5. date请存放文章日期
6. summary文件保存文章简述，用于文章列表中展示
7. tags文件为文章的tag列表
8. title为文章标题
9. 把文件夹放到根目录的Blogs文件夹中即可