
function Publish(islogn) {
    if (islogn) {
        var Publish_btn = document.getElementById("Publish_btn");
        Publish_btn.style.display = "block";
    }
}
function notPublish() {
    var Publish_btn = document.getElementById("Publish_btn");
    var Publish_text = document.getElementById("publish_text");
    if (Publish_text.value == "")
        Publish_btn.style.display = "none";

}
function TextChange(obj) {
    if (obj.value.length > 200) {
        obj.value = obj.value.substr(0, 200);
        alert("输入字数超限，评论字数请小于200字！");
    }
}
function Publish_click(id) {
    var Publish_text = document.getElementById("publish_text");
    if (Publish_text.value != null)
        StartAjax("post", "Publish.ashx", true, "req=1&request=" + document.getElementById("publish_text").value + "&ID="+id);
    else
        alert("不能发表空的内容！ ");
}

function CallbackSuccess() {
    var Publish_btn = document.getElementById("Publish_btn"); 
    var string = xmlHttpRequest.responseText;
    var res = eval("(" + string + ")"); 
    if (res.request == "OK" && res.Handle == "Review")
    {
        var panel = document.getElementById("panel");
        var Publish_text = document.getElementById("publish_text");
        var Publish_btn = document.getElementById("Publish_btn");
        panel.innerHTML = "<canvas id=\"newcanves\" class=\"can\" width=\"100\" height=\"100\" title=\""+res.time+"\"></canvas><div class=\"alert alert-info\"><p>"+res.username+" 说：<br />" + Publish_text.value + "<span onclick=\"Reply();\">回复</span><b class=\"span\">"+res.time+"</b></p></div>" + panel.innerHTML;
        draw();
        hua(res.time);
        Publish_btn.style.display = "none";
        Publish_text.value = "";

    }
    else if (res.request == "OK" && res.Handle == "Reply")
    {
        ReplyPanel(res); 
    }
    else if (res.request == "OK" && res.Handle == "ReplyQT")
    {
        var div1 = document.getElementById("div");
        var t = document.getElementById("t");
        var str = "<div style=\"margin-left:108px\"><h5 style='color:#379BE9'>" + res.username + "&nbsp&nbsp说：</h5>&nbsp&nbsp&nbsp&nbsp" + t.value + "<hr /></div>";
        t.value = "";
        t.style.display = "none";
        div1.innerHTML = str + div1.innerHTML;
        
    }
    else if (res.request == "ON") {
        alert("请不要输入HTML或javascript文本！");
    }
    else if (res.request == "Warning")
    {
        alert("不能发表空的评论！");
    }
    else {
        alert("系统发生未知错误！请重试...");
    }
}

function draw() {
    var c = document.getElementsByClassName("can");
    for (var i = 0; i < c.length; i++) {
        var cxt = c[i].getContext("2d");
        cxt.fillStyle = "#9DCEDF";
        cxt.beginPath();
        cxt.arc(50, 50, 48, 0, Math.PI * 2, true);
        cxt.closePath();
        cxt.fill();

        cxt.fillStyle = "#A8DCEE";
        cxt.moveTo(20, 64);
        cxt.lineTo(80, 64);
        cxt.lineTo(80, 66);
        cxt.lineTo(20, 66);
        cxt.closePath();
        cxt.fill();

        var Year = c[i].title.substring(0, 4);
        var Month = c[i].title.substring(6, 8);
        var Day = c[i].title.substring(8, 10);

        switch (Month) {

            case "1-": Month = "January"; break;
            case "2-": Month = "February"; break;
            case "3-": Month = "March"; break;
            case "4-": Month = "April"; break;
            case "5-": Month = "May"; break;
            case "6-": Month = "June"; break;
            case "7-": Month = "July"; break;
            case "8-": Month = "August"; break;
            case "9-": Month = "September"; break;
            case "10": Month = "October"; break;
            case "11": Month = "November"; break;
            case "12": Month = "December"; break;
        }

        cxt.font = "20px Times New Roman";
        cxt.fillStyle = "#000"
        cxt.fillText(Year, 30, 80);

        cxt.font = "40px Times New Roman";
        cxt.fillStyle = "#000"
        cxt.fillText(Day, 30, 60);

        cxt.font = "20px Times New Roman";
        cxt.fillStyle = "#000"
        cxt.fillText(Month, 27, 30);
    }
}
function CreateFailure() {
    alert("发表评论失败，请重试！");
}
function SendFailure() {
    alert("发表评论失败，请检查你的网络！");
}
function CallbackFailure() {
    alert("发表评论失败，请检查你的网络！");
}
function hua(time) {
    var Year = time.substring(0, 4);
    var Month = time.substring(6, 8);
    var Day = time.substring(8, 10);

    switch (Month) {

        case "1-": Month = "January"; break;
        case "2-": Month = "February"; break;
        case "3-": Month = "March"; break;
        case "4-": Month = "April"; break;
        case "5-": Month = "May"; break;
        case "6-": Month = "June"; break;
        case "7-": Month = "July"; break;
        case "8-": Month = "August"; break;
        case "9-": Month = "September"; break;
        case "10": Month = "October"; break;
        case "11": Month = "November"; break;
        case "12": Month = "December"; break;
    }
    var c = document.getElementById("newcanves");
    var cxt = c.getContext("2d");
    cxt.font = "20px Times New Roman";
    cxt.fillStyle = "#000"
    cxt.fillText(Year, 30, 80);

    cxt.font = "40px Times New Roman";
    cxt.fillStyle = "#000"
    cxt.fillText(Day, 30, 60);

    cxt.font = "20px Times New Roman";
    cxt.fillStyle = "#000"
    cxt.fillText(Month, 27, 30);

}
var time = null;
var i = 1;
var id = null;
function Reply(ID,islogin,GoodID)
{
   
    var Coverage = document.getElementById("Coverage");
    var Reply = document.getElementById("Reply");
    var str = document.getElementById(ID).innerText || document.getElementById(ID).textContent;
    id = ID;
    Coverage.style.Width = document.body.scrollWidth + "px";
    Coverage.style.height = document.body.scrollHeight +"px" ;
    Coverage.style.background = "#808080";
    Coverage.style.display = "block";
    Reply.style.top ="-"+ 500 + "px";
    Reply.style.left = (document.body.scrollLeft || document.documentElement.scrollLeft) + (document.body.offsetWidth-793)/2 + "px";
    i = 1;
    time=setInterval("move()", 10);

    StartAjax("post", "Publish.ashx", true, "req=2&ID=" + ID);
  
    Reply.style.display = "block";
    var Username = str.split("说：");
    var string = Username[1].split("回复");
    var content = string[0]
    for (var i = 1; i < string.length - 1; i++)
    {
       content=content+string[i]
    }
    var str = "<div id=\"speed\"><div id=\"speed_div\" style=\"margin:0px;\"></div></div> <div style=\"margin-top:25px;margin-left:68px;word-break:break-all;\"><h3 style='color:#379BE9'>" + Username[0] + "说：</h3>&nbsp&nbsp&nbsp&nbsp" + content + "<span style=\"color:#2fae16;cursor:pointer;float:right;margin-right:40px;\" onclick=\"ReplyAgain(" + ID + "," + islogin + "," + GoodID + ");\">我要回复</span></div><hr />";
    Reply.innerHTML =str;
}



function move()
{
    var Reply = document.getElementById("Reply");
    Reply.style.top =(i-500)+ "px";
    i=i+20;
    if (i >= 600) {
        clearInterval(time);
        i = 1;
    }
}

var Panel = false;

function Close() {
    var Reply = document.getElementById("Reply");
    var Coverage = document.getElementById("Coverage");
    Reply.style.display = "none";
    Coverage.style.display = "none";
    Panel = false;
}
function ReplyPanel(res)
{
    var Reply = document.getElementById("Reply");
    var str = "<div  style=\"height:400px;overflow:hidden;\"><div id=\"div\" style=\"margin-top:0px;height:auto;\">";
    for (var i = 0; i <= parseInt(res.number)-1 ; i++)
    {
        str = str + "<div style=\"margin-left:108px\"><h5 style='color:#379BE9'>" + res.table[i].Username + "说：</h5>&nbsp&nbsp&nbsp&nbsp" + res.table[i].Content + "<hr /></div>";
    }
    Reply.innerHTML = Reply.innerHTML + (str + "</div></div>");
    var speed = document.getElementById("speed_div");
    if (parseInt(document.getElementById("div").offsetHeight) < 380) {
        speed.style.height = "380px";
    }
   else
        speed.style.height = parseInt((380 * 345) / (parseInt(document.getElementById("div").offsetHeight))) + "px";

}


function ReplyAgain(ID, islogin, GoodID) {
    if (islogin) {
        if (!Panel) {
            var div1 = document.getElementById("div");
            div1.style.marginTop = 0;
            div.innerHTML = "<textarea id='t' class=\"form-control\" rows=\"3\" cols=\"6\"  onkeydown=\"Submit(event," + GoodID + ");\" onkeyup=\"TextChange(this);\" placeholder=\"回车就能发送哦\" ></textarea>" + div1.innerHTML;
            Panel = true;
        }
    }
    else
    {
        alert("登录才能发表评论！");
    }
}
function Submit(e,GoodID)
{
    var eve = e || window.event;
    var t = document.getElementById("t");
    var keynum = e.keyCode;
    if (keynum == 13) {
        strHtml=t.value;
        strHtml = strHtml.replace(/\r\n/g, "<br />");
        strHtml = strHtml.replace(/\n/g, "<br />");
        if (strHtml.length > 0&&strHtml!="") {
            isReplyWindow = false;
            StartAjax("post", "Publish.ashx", true, "req=3&ID=" + id + "&GoodID=" + GoodID + "&request=" + strHtml);
        }
        else {
            alert("不能发表空回复！");
        }
    }
}

function openpublish(id)
{
    window.open("Review.ashx?ID="+id+"&pag=1");
}