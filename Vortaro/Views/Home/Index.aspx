<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    主页
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../Content/Jquery-easyui-1.3.2/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../../Content/Jquery-easyui-1.3.2/themes/icon.css" rel="stylesheet" type="text/css" />
     <link href="../../Content/Index.css" rel="stylesheet" type="text/css" />
    <script src="../../Content/Jquery-easyui-1.3.2/jquery-1.8.0.min.js" type="text/javascript"></script>
    <script src="../../Content/Jquery-easyui-1.3.2/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../../Content/Jquery-easyui-1.3.2/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <script src="../../Content/Home/Index.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div id="tabs-Project" class="easyui-tabs" data-options="fit:true,border:true,tabPosition:'left'" style="height:460px">  
        <div title="项目" style="padding:5px;" data-options="iconCls:'icon-projects'">
            <table id="dg-Project"></table>
        </div>
        <div title="数据库" style="padding:5px;" data-options="iconCls:'icon-database'">
            <div class="easyui-layout" data-options="fit:true,border: false">
			    <div data-options="region:'center'">
                    <table id="dg-Database"></table>  
			    </div>
		    </div>
        </div>
        <div title="分组" style="padding:5px;" data-options="iconCls:'icon-tickets'">
            <div class="easyui-layout" data-options="fit:true,border: false">
			    <div data-options="region:'center'">
                    <table id="dg-Group"></table>  
			    </div>
		    </div>
        </div>
        <div title="表字段" style="padding:5px;" data-options="iconCls:'icon-table'">
            <div class="easyui-layout" data-options="fit:true,border: false">
			    <div data-options="region:'west',split:true" style="width:600px;">
                    <table id="dg-Tables"></table>
                </div>
			    <div data-options="region:'center'">
				    <table id="dg-Column"></table>
			    </div>
		    </div>
        </div>  
    </div>
    <div id="window-Project" iconCls="icon-book">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false">
                <div style="height:auto;clear:both;">
	                <label for="project_Name" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">名称:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="project_Name" style="height:35px; margin-left:5px;">
		                    <input id="txtProjectName" style="height:18px; margin-top:5px;border-radius: 5px;"  name="prt_ProjectName" type="text"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="project_Bewrite" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">描述:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="project_Bewrite" style="height:35px; margin-left:5px;">
		                    <input id="txtProjectBewrite" style="height:18px; margin-top:5px;border-radius: 5px;"  name="prt_ProjectBewrite" type="text"/>
		                </div>
	                </div>
                </div>
            </div>
            <div data-options="region:'south',border:false" style="text-align:center; padding-top:10px; padding-bottom:10px;">  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-save'" href="javascript:void(0)" onclick="javascript:Port.SaveProject();">保存</a>  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="javascript:void(0)" onclick="javascript:$('#window-Project').window('close');">取消</a>  
            </div>
        </div>
    </div>
    <div id="window-Database" iconCls="icon-book">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false">
                <div style="height:auto;clear:both;">
	                <label for="database_Name" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">名称:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div2" style="height:35px; margin-left:5px;">
		                    <input id="txtDatabaseName" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_Name" type="text"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="database_Alias" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">别名:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div3" style="height:35px; margin-left:5px;">
		                    <input id="txtDatabaseAlias" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_Alias" type="text"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="database_Bewrite" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">描述:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div5" style="height:35px; margin-left:5px;">
		                    <input id="txtDatabaseBewrite" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_Bewrite" type="text" />
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="database_Type" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">类型:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div1" style="height:35px; margin-left:5px;">
		                    <input id="txtDatabaseType" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_Type" type="text" value="MSSQLSERVER"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="database_ServerName" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">服务器名称:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div4" style="height:35px; margin-left:5px;">
		                    <input id="txtServerName" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_ServerName" type="text" value=""/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="database_ServerUser" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">登录名:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div6" style="height:35px; margin-left:5px;">
		                    <input id="txtServerUser" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_ServerUser" type="text" value=""/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="database_ServerPwd" style="width:95px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">密码:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div7" style="height:35px; margin-left:5px;">
		                    <input id="txtServerPwd" style="height:18px; margin-top:5px;border-radius: 5px;"  name="database_ServerPwd" type="text" value=""/>
		                </div>
	                </div>
                </div>
            </div>
            <div data-options="region:'south',border:false" style="text-align:center; padding-top:10px; padding-bottom:10px;">
                <a class="easyui-linkbutton" data-options="iconCls:'icon-link'" href="javascript:void(0)" onclick="javascript:Port.ConnectionDatabase();">测试</a>
                <a id="window-Database-Save" class="easyui-linkbutton" data-options="iconCls:'icon-save'" href="javascript:void(0)" onclick="javascript:Port.SaveDatabase();">保存</a>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="javascript:void(0)" onclick="javascript:$('#window-Database').window('close');">取消</a>  
            </div>
        </div>
    </div>
    <div id="window-Group" iconCls="icon-book">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false">
                <div style="height:auto;clear:both;">
	                <label for="group_Name" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">名称:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="group_Name" style="height:35px; margin-left:5px;">
		                    <input id="txtGroupName" style="height:18px; margin-top:5px;border-radius: 5px;"  name="group_Name" type="text"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="group_Bewrite" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">描述:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="group_Bewrite" style="height:35px; margin-left:5px;">
		                    <input id="txtGroupBewrite" style="height:18px; margin-top:5px;border-radius: 5px;"  name="group_Bewrite" type="text"/>
		                </div>
	                </div>
                </div>
            </div>
            <div data-options="region:'south',border:false" style="text-align:center; padding-top:10px; padding-bottom:10px;">  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-save'" href="javascript:void(0)" onclick="javascript:Port.SaveGroup();">保存</a>  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="javascript:void(0)" onclick="javascript:$('#window-Group').window('close');">取消</a>  
            </div>
        </div>
    </div>
    <div id="window-Tables" iconCls="icon-book">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false">
                <div style="height:auto;clear:both;">
	                <label for="group_Name" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">名称:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div9" style="height:35px; margin-left:5px;">
		                    <input id="txtTablesName" style="height:18px; margin-top:5px;border-radius: 5px;"  name="tables_Name" type="text"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="group_Bewrite" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">别名:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div10" style="height:35px; margin-left:5px;">
		                    <input id="txtTablesAlias" style="height:18px; margin-top:5px;border-radius: 5px;"  name="tables_Alias" type="text"/>
		                </div>
	                </div>
                </div>
            </div>
            <div data-options="region:'south',border:false" style="text-align:center; padding-top:10px; padding-bottom:10px;">  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-save'" href="javascript:void(0)" onclick="javascript:Port.SaveTable();">保存</a>  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="javascript:void(0)" onclick="javascript:$('#window-Tables').window('close');">取消</a>  
            </div>
        </div>
    </div>
    <div id="window-Import-Table">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false">
                <div id="Import-Grid"></div>
            </div>
            <div data-options="region:'south',border:false" style="text-align:center; padding-top:10px; padding-bottom:10px;">  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-arrow-downward'" href="javascript:void(0)" onclick="javascript:Port.ImportTable();">导入</a>
                <a class="easyui-linkbutton" data-options="iconCls:'icon-config'" href="javascript:void(0)" onclick="javascript:Port.ImportTableAll(this);">全选</a>  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-reload'" href="javascript:void(0)" onclick="javascript:Port.ImportTableRefresh();">刷新</a>  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="javascript:void(0)" onclick="javascript:$('#window-Import-Table').window('close');">取消</a>  
            </div>
        </div>
    </div>
    <div id="window-Column" iconCls="icon-book">
        <div class="easyui-layout" data-options="fit:true">
            <div data-options="region:'center',border:false">
                <div style="height:auto;clear:both;">
	                <label for="column_Bewrite" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">前缀:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div14" style="height:35px; margin-left:5px;">
		                    <input id="txtColumnOwner" style="height:18px; margin-top:5px;border-radius: 5px;"  name="column_Owner" type="text" value="dbo"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="column_Name" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">列名:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div11" style="height:35px; margin-left:5px;">
		                    <input id="txtColumnName" style="height:18px; margin-top:5px;border-radius: 5px;"  name="column_Name" type="text"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="column_Type" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">类型:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div8" style="height:35px; margin-left:5px;">
                            <input id="txtColumnType" name="column_Type" class="easyui-combobox" data-options="valueField:'id',textField:'text',width:200"/>
		                </div>
	                </div>
                </div>
                <div style="height:auto;clear:both;">
	                <label for="column_Bewrite" style="width:55px;height:35px;text-align:right;line-height:20px;padding-top:20px;padding-top:7px;font-size:14px;font-weight:bold;float:left;">说明:</label>
	                <div style="float:left;position:relative;width:210px;height:35px;">
		                <div id="Div13" style="height:35px; margin-left:5px;">
		                    <input id="txtColumnBewrite" style="height:18px; margin-top:5px;border-radius: 5px;"  name="column_Bewrite" type="text"/>
		                </div>
	                </div>
                </div>
            </div>
            <div data-options="region:'south',border:false" style="text-align:center; padding-top:10px; padding-bottom:10px;">  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-save'" href="javascript:void(0)" onclick="javascript:Port.SaveColumn();">保存</a>  
                <a class="easyui-linkbutton" data-options="iconCls:'icon-cancel'" href="javascript:void(0)" onclick="javascript:$('#window-Column').window('close');">取消</a>  
            </div>
        </div>
    </div>
</asp:Content>
