define(function (require) {
    var dependencies = {
        'footer': require('./footer')
    };
    var helpers = require('../$helpers');
    var $render = function (id, data) {
            return dependencies[id](data);
        };
    var Render = function ($data) {
            'use strict';
            var $helpers = this,
                $escape = $helpers.$escape,
                $string = $helpers.$string,
                Alias = $data.Alias,
                Name = $data.Name,
                i = $data.i,
                list = $data.list,
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
            $out += '<div class="panel" style="min-height:600px;" > <div class="wrapper" style="width:95%;" > <div style="text-align:center;"><b>';
            $out += $escape($string(Alias));
            $out += '（';
            $out += $escape($string(Name));
            $out += '）</b></div><br/> <div> <table style="width: 100%;"> <tr class="text-center"> <th style="width:60px">序号</th> <th>字段</th> <th>类型</th> <th>说明</th> <th>作者</th> <th>时间</th> </tr> ';
            for (i = 0; i < list.length; i++) {
                $out += ' <tr> <td style="padding-left:10px;">';
                $out += $escape($string(i + 1));
                $out += '</td> <td>';
                $out += $escape($string(list[i].Name));
                $out += '</td> <td>';
                $out += $escape($string(list[i].Type));
                $out += '</td> <td>';
                $out += $escape($string(list[i].Bewrite));
                $out += '</td> <td class="text-center">';
                $out += $escape($string(list[i].Author));
                $out += '</td> <td class="text-center">';
                $out += $escape($string(list[i].CreateTime));
                $out += '</td> </tr> ';
            }
            $out += ' </table> </div> <div style="padding-top:20px;text-align:center;"> <a href="javascript:;" onclick="javascript:history.go(-1);">返回</a> </div> </div> </div> ';
            include('footer')
            return new String($out)
        };
    Render.prototype = helpers;
    return function (data) {
        helpers.$render = $render;
        return new Render(data) + '';
    }
});