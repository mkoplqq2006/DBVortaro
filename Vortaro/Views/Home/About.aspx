<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="aboutTitle" ContentPlaceHolderID="TitleContent" runat="server">
    关于DBVortaro
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="width:100%">
        <h2>关于DBVortaro</h2>
        <p>如果你对DBVortaro有什么意见建议可以通过在线咨询或邮箱的联系方式找到作者。<br/>
        DBVortaro一直在不断完善自身，这个愉悦的过程中感谢有你的参与～<br/><br/>
        提交BUG注意事项：<br/>
        1、浏览器名称以及版本。 <br/>
        2、简单的描述信息 <br/>
        3、建议提取一份BUG文件(bin/log-error.config)发送给作者，这将会使我们能更好的解决问题。<br/><br/>
        您因使用DBVortaro而受益或者感到愉悦，您还可以这样帮助DBVortaro成长：<br/>
        1、共同参与并完善DBVortaro或用blog/微博/Twitter把它分享它给更多的人。<br/>
        2、DBVortaro有幸被用于管理您的项目字典，请您联系我，我将在DBVortaro后期版本中展示您项目/企业的LOGO。<br/><br/>
        在线咨询：<a onclick="var tempSrc='http://sighttp.qq.com/wpa.js?rantime='+Math.random()+'&amp;sigkey=383dac81f84f086691f54b0d510b7ae2fd2e0d14dfd0a62ef6cb3009e257eede';var oldscript=document.getElementById('testJs');var newscript=document.createElement('script');newscript.setAttribute('type','text/javascript'); newscript.setAttribute('id', 'testJs');newscript.setAttribute('src',tempSrc);if(oldscript == null){document.body.appendChild(newscript);}else{oldscript.parentNode.replaceChild(newscript, oldscript);}return false;" href="http://sighttp.qq.com/cgi-bin/check?sigkey=383dac81f84f086691f54b0d510b7ae2fd2e0d14dfd0a62ef6cb3009e257eede" target="_blank"><img border="0" alt="有事请联系我" src="http://wpa.qq.com/pa?p=1:330906531:42"/></a><br/>
        作者：张德兵<br/>
        邮箱：1394591229@qq.com (恕不提供使用问题咨询，请认真阅读使用说明)<br/>
        微博：<a href="http://weibo.com/u/1936603442" target="_blank">满月十五</a>(新浪) <a href="http://t.qq.com/zhangdebing" target="_blank">zhangdebing</a>(腾讯)<br/><br/>
        捐赠: <a href='http://me.alipay.com/mkoplqq2006' target="_blank"><img alt="捐赠共勉" src='../../Content/Images/donate.png' border="0" /></a><br/>
        </p>
    </div>
</asp:Content>
