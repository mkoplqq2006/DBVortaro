define(function (require) {
    var helpers = require('../$helpers');
    var Render = function ($data) {
            'use strict';
            var $helpers = this,
                $escape = $helpers.$escape,
                $string = $helpers.$string,
                copyright = $data.copyright,
                $out = '';
            $out += '<div style="text-align:center;height:50px;"> 作者：';
            $out += $escape($string(copyright));
            $out += ' </div>';
            return new String($out)
        };
    Render.prototype = helpers;
    return function (data) {
        return new Render(data) + '';
    }
});