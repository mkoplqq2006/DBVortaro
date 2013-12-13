define(function (require) {
    var dependencies = {
        './Items/footer': require('./Items/footer')
    };
    var helpers = require('./$helpers');
    var $render = function (id, data) {
            return dependencies[id](data);
        };
    var Render = function ($data) {
            'use strict';
            var $helpers = this,
                $escape = $helpers.$escape,
                $string = $helpers.$string,
                projectName = $data.projectName,
                j = $data.j,
                list = $data.list,
                childrenList = $data.childrenList,
                i = $data.i,
                include = function (id, data) {
                    if (data === undefined) {
                        data = $data
                    }
                    var content = $helpers.$render(id, data);
                    if (content !== undefined) {
                        $out += content;
                        return content
                    }
                },
                $out = '';
            $out += '<div class="panel" style="min-height:600px;" > <div class="wrapper" style="min-height:400px;width:95%;" > <div style="text-align:center;color:#555;"><b>项目：';
            $out += $escape($string(projectName));
            $out += '</b></div><br/> <div> ';
            for (j = 0; j < list.length; j++) {
                $out += ' <table style="width: 100%;"> <tr> <td colspan="5" style="font-weight:bold;line-height:20px;"> 服务器：';
                $out += $escape($string(list[j].serverName));
                $out += '<br/> 数据库：';
                $out += $escape($string(list[j].databaseName));
                $out += ' </td> </tr> <tr class="text-center"> <th style="width:60px">序号</th> <th>分组</th> <th>表名</th> <th>别名</th> <th>作者</th> </tr> ';
                var childrenList = list[j].list;
                for (i = 0; i < childrenList.length; i++) {

                    $out += ' <tr> <td style="padding-left:10px;">';
                    $out += $escape($string(i + 1));
                    $out += '</td> <td class="text-center">';
                    $out += $escape($string(childrenList[i].GroupName));
                    $out += '</td> <td> <a href="';
                    $out += $escape($string(childrenList[i].Url));
                    $out += '">';
                    $out += $escape($string(childrenList[i].Name));
                    $out += '</a> </td> <td> <a href="';
                    $out += $escape($string(childrenList[i].Url));
                    $out += '">';
                    $out += $escape($string(childrenList[i].Alias));
                    $out += '</a> </td> <td class="text-center">';
                    $out += $escape($string(childrenList[i].Author));
                    $out += '</td> </tr> ';
                }
                $out += ' </table> <br /> ';
            }
            $out += ' </div> </div> </div> ';
            include('./Items/footer')
            return new String($out)
        };
    Render.prototype = helpers;
    return function (data) {
        helpers.$render = $render;
        return new Render(data) + '';
    }
});