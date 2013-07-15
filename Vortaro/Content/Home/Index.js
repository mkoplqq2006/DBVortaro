//关闭AJAX相应的缓存
$.ajaxSetup({
    cache: false 
});
var index = 0,//项目选中行
	typeCmd='1',//数据处理类型('1'创建，'2'编辑)
	projectCode='',//选择的项目编码
	projectName='',//选择的项目名称
	databaseCode='',//选择的数据库编码
	databaseName='',//选择的数据库名称
	groupCode='',//选择的功能分组编码
	tablesCode='',//表编码
	tablesName='',//表名称
	ServerName='',//服务器名称
	ServerUser='',//登录名
	ServerPwd='',//密码
	tabsIndex=0;//面板切换序号
$(document).ready(function(){
	var pageSize=30,query='';
	//项目window
	$('#window-Project').window({   
		title:'项目',
		width:315, 
		height:175,
		minimizable:false,
		maximizable:false,
		collapsible:false,
		modal:true,
		resizable:false,
		closed:true
	}); 
	//项目toolbar
	var projectToolbar = [{
		text:'创建',
		iconCls:'icon-add',
		handler:function(){
			typeCmd = '1';
			$('#window-Project').window('open');
			$('#txtProjectName').val('');
			$('#txtProjectBewrite').val('');
		}
	},{
		text:'编辑',
		iconCls:'icon-edit',
		handler:function(){
			var row = $('#dg-Project').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择要编辑的项目信息！','warning');
				return;
			}
			typeCmd = '2';
			$('#window-Project').window('open');
			$('#txtProjectName').val(row.Name);
			$('#txtProjectBewrite').val(row.Bewrite);
		}
	},{
		text:'移除',
		iconCls:'icon-remove',
		handler:function(){
			Port.DelProject();
		}
	},{
		text:'生成字典',
		iconCls:'icon-publish',
		handler:function(){
			var row = $('#dg-Project').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择需要生成的项目！','warning');
				return;
			}
			$.ajax({
				url: '/Project/PublishDictionary',
				type: 'POST',
				data: {
					projectCode:row.Code,
					projectName:row.Name
				},
				success: function (data) {
					if(data == ''){return;}
					var json = eval('(' + data + ')');
					if (!json.HasError) {
						$.messager.alert('提示','字典生成成功！');
					} else {
						$.messager.alert('提示',json.msg,'warning');
					}
				},
				error: function () {
					$.messager.alert('提示','服务器忙！','error');
				}
			});
		}
	},{
		text:'打包字典',
		iconCls:'icon-rar',
		handler:function(){
			var row = $('#dg-Project').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择需要打包的项目！','warning');
				return;
			}
			/*创建form请求*/
			var form = $('<form>');  
			form.attr('style','display:none');  
			form.attr('target','');  
			form.attr('method','post');  
			form.attr('action','/Project/UnpackDictionary');
			var Code = $('<input>');
			Code.attr('type','text');
			Code.attr('name','projectCode');//项目编码
			Code.attr('value',row.Code);
			var Name = $('<input>');
			Name.attr('type','text');
			Name.attr('name','projectName');//项目名称
			Name.attr('value',row.Name);
			$('body').append(form);
			form.append(Code);
			form.append(Name);
			form.submit();  
			form.remove();
		}
	},{
		text:'预览字典',
		iconCls:'icon-view',
		handler:function(){
			window.open('../../Content/Dictionary/Preview/index.html?code='+projectCode+'&name='+escape(projectName),'_blank');
		}
	}];
	//项目grid
	$('#dg-Project').datagrid({
		title:'项目',
		url:'/Project/GetPageProject',
		queryParams: {
			query:query
		},
		pageSize:pageSize,
		pagination: true,
		rownumbers: true,
		fitColumns: true,
		singleSelect: true,
		remoteSort:false,
		fit: true,          
		striped: true,
		toolbar:projectToolbar,		
		columns:[[
			{field:'Name',title: '项目名称',width: 50,sortable:true},
			{field:'Bewrite',title: '描述',width: 50},
			{field:'Author',title: '作者',width: 50,hidden:true},
			{field:'CreateTime',title: '创建时间',width: 160,sortable:true,hidden:true}
		]],
		onClickRow:function(rowIndex, rowData){
			index = rowIndex;//得到选中行
			projectCode = rowData.Code;
			projectName = rowData.Name;
			$('#tabs-Project').tabs('enableTab', 1);
			$('#tabs-Project').tabs('enableTab', 2);
			//加载功能分组grid
			$('#dg-Group').datagrid('load',{
				projectCode:projectCode,
				query:query
			});
			//数据库grid
			$('#dg-Database').datagrid('load',{
				projectCode:projectCode,
				query:query
			});
		}
	});
    //项目pager
	$('#dg-Project').datagrid('getPager').pagination({
		pageSize: pageSize,
		pageList: [30,60,90,120,150],
		onBeforeRefresh:function(pageNumber, pageSize){
			$(this).pagination('loading');
			$(this).pagination('loaded');
		}
	});

	//功能分组window
	$('#window-Group').window({
		title:'功能分组',
		width:315, 
		height:175,
		minimizable:false,
		maximizable:false,
		collapsible:false,
		modal:true,
		resizable:false,
		closed:true
	}); 
	//功能分组toolbar
	var groupToolbar = [{
		text:'创建',
		iconCls:'icon-add',
		handler:function(){
			typeCmd = '1';
			$('#window-Group').window('open');
			$('#txtGroupName').val('');
			$('#txtGroupBewrite').val('');
		}
	},{
		text:'编辑',
		iconCls:'icon-edit',
		handler:function(){
			var row = $('#dg-Group').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择要编辑的功能分组信息！','warning');
				return;
			}
			typeCmd = '2';
			$('#window-Group').window('open');
			$('#txtGroupName').val(row.Name);
			$('#txtGroupBewrite').val(row.Bewrite);
		}
	},{
		text:'移除',
		iconCls:'icon-remove',
		handler:function(){
			Port.DelGroup();
		}
	}];
	//功能分组grid
	$('#dg-Group').datagrid({
		title:'功能分组',
		url:'/Group/GetPageGroup',
		queryParams: {
			projectCode:projectCode,
			query:query
		},
		pageSize:pageSize,
		pagination: true,
		rownumbers: true,
		fitColumns: true,
		singleSelect: true,
		remoteSort:false,
		fit: true,
		border: false,            
		striped: true,
		toolbar:groupToolbar,		
		columns:[[
			{field:'Name',title: '分组名称',width: 50,sortable:true},
			{field:'Bewrite',title: '描述',width: 50},
			{field:'Author',title: '作者',width: 50,hidden:true},
			{field:'CreateTime',title: '创建时间',width: 160,sortable:true,hidden:true}
		]],
		onClickRow:function(rowIndex, rowData){
			index = rowIndex;//得到选中行
			groupCode = rowData.Code;
			if(databaseCode != ''){
				$('#tabs-Project').tabs('enableTab', 3);
				//表grid
				$('#dg-Tables').datagrid('load',{
					databaseCode:databaseCode,
					groupCode:groupCode,
					query:query
				});
				//清空字段grid
				$('#dg-Column').datagrid('loadData', { total: 0, rows: [] });
			}
		}
	});
	//功能分组pager
	$('#dg-Group').datagrid('getPager').pagination({
		pageSize: pageSize,
		pageList: [30,60,90,120,150],
		onBeforeRefresh:function(pageNumber, pageSize){
			$(this).pagination('loading');
			$(this).pagination('loaded');
		}
	});
	
	//数据库window
	$('#window-Database').window({
		title:'数据库',
		width:345, 
		height:390,
		minimizable:false,
		maximizable:false,
		collapsible:false,
		modal:true,
		resizable:false,
		closed:true
	});
	//数据库toolbar
	var databaseToolbar = [{
		text:'创建',
		iconCls:'icon-add',
		handler:function(){
			typeCmd = '1';
			$('#window-Database').window('open');
			$('#txtDatabaseName').val('');
			$('#txtDatabaseAlias').val('');
			$('#txtDatabaseBewrite').val('');
			$("#txtServerName").val('');
			$("#txtServerUser").val('');
			$("#txtServerPwd").val('');
		}
	},{
		text:'编辑',
		iconCls:'icon-edit',
		handler:function(){
			var row = $('#dg-Database').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择要编辑的数据库信息！','warning');
				return;
			}
			typeCmd = '2';
			$('#window-Database').window('open');
			$('#txtDatabaseName').val(row.Name);
			$('#txtDatabaseAlias').val(row.Alias);
			$('#txtDatabaseBewrite').val(row.Bewrite);
			$('#txtDatabaseType').val(row.Type);
			$("#txtServerName").val(row.ServerName);
			$("#txtServerUser").val(row.ServerUser);
			$("#txtServerPwd").val(row.ServerPwd);
		}
	},{
		text:'移除',
		iconCls:'icon-remove',
		handler:function(){
			Port.DelDatabase();
		}
	}];
	//数据库grid
	$('#dg-Database').datagrid({
		title:'数据库',
		url:'/Database/GetPageDatabase',
		queryParams: {
			projectCode:projectCode,
			query:query
		},
		pageSize:pageSize,
		pagination: true,
		rownumbers: true,
		fitColumns: true,
		singleSelect: true,
		remoteSort:false,
		fit: true,
		border: false,            
		striped: true,
		toolbar:databaseToolbar,		
		columns:[[
			{field:'Name',title: '数据库名称',width: 80,sortable:true},
			{field:'Alias',title: '数据库别名',width: 80},
			{field:'Type',title: '数据库类型',width: 80},
			{field:'ServerName',title: '服务器名称',width: 70},
			{field:'Bewrite',title: '描述',width: 50},
			{field:'Author',title: '作者',width: 50,hidden:true},
			{field:'CreateTime',title: '创建时间',width: 160,sortable:true,hidden:true}
		]],
		onClickRow:function(rowIndex, rowData){
			index = rowIndex;//得到选中行
			databaseCode = rowData.Code;
			databaseName=rowData.Name;
			ServerName=rowData.ServerName;
			ServerUser=rowData.ServerUser;
			ServerPwd=rowData.ServerPwd;
			if(groupCode != ''){
				$('#tabs-Project').tabs('enableTab', 3);
				//表grid
				$('#dg-Tables').datagrid('load',{
					databaseCode:databaseCode,
					groupCode:groupCode,
					query:query
				});
				//清空字段grid
				$('#dg-Column').datagrid('loadData', { total: 0, rows: [] });
			}
		}
	});
	//数据库pager
	$('#dg-Database').datagrid('getPager').pagination({
		pageSize: pageSize,
		pageList: [30,60,90,120,150],
		onBeforeRefresh:function(pageNumber, pageSize){
			$(this).pagination('loading');
			$(this).pagination('loaded');
		}
	});
	
	//导入表
	$('#window-Import-Table').window({
		title:'导入表',
		iconCls:'icon-arrow-downward',
		width:567, 
		height:390,
		minimizable:false,
		collapsible:false,
		modal:true,
		resizable:false,
		closed:true
	});

	//表window
	$('#window-Tables').window({
		title:'表',
		width:345, 
		height:175,
		minimizable:false,
		maximizable:false,
		collapsible:false,
		modal:true,
		resizable:false,
		closed:true
	});
	//表toolbar
	var tablesToolbar = [{
		text:'导入',
		iconCls:'icon-arrow-downward',
		handler:function(){
			var row = $('#dg-Database').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择数据库！','warning');
				return;
			}
			//导入表Grid
			$('#Import-Grid').datagrid({
				url:'/Tables/GetImportTables',
				queryParams: {
					DatabaseCode:databaseCode,
					DatabaseName:databaseName,
					ServerName:ServerName,
					ServerUser:ServerUser,
					ServerPwd:ServerPwd
				},
				rownumbers: true,
				fitColumns: true,
				remoteSort:false,
				fit: true,
				border: false,            
				striped: true,	
				columns:[[
					{field:'name',title: '表名',width: 300,sortable:true,formatter:Port.TableIcon},
					{field:'createdate',title: '创建时间',width: 160,sortable:true}
				]]
			});
			var row2 = $('#Import-Grid').datagrid('getRows');
			$('#window-Import-Table').window('open');
		}
	},{
		text:'创建新表',
		iconCls:'icon-add',
		handler:function(){
			typeCmd = '1';
			$('#window-Tables').window('open');
			$('#txtTablesName').val('');
			$('#txtTablesAlias').val('');
		}
	},{
		text:'编辑表',
		iconCls:'icon-edit',
		handler:function(){
			var row = $('#dg-Tables').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择要编辑的数据库信息！','warning');
				return;
			}
			typeCmd = '2';
			$('#window-Tables').window({
				title:'表 '+row.Name
			});
			$('#window-Tables').window('open');
			$('#txtTablesName').val(row.Name);
			$('#txtTablesAlias').val(row.Alias);
		}
	},{
		text:'移除表',
		iconCls:'icon-remove',
		handler:function(){
			Port.DelTable();
		}
	},{
		text:'全选',
		iconCls:'icon-config',
		handler:function(){
			if($.browser.msie) {
				Port.ImportBtnSelete(this,this.innerText,$('#dg-Tables'));
			}else{
				Port.ImportBtnSelete(this,this.textContent,$('#dg-Tables'));
			}
		}
	},{
		text:'同步',
		iconCls:'icon-reload',
		handler:function(){
			var row = $('#dg-Tables').datagrid('getSelections');
			if(row.length == 0){
				$.messager.alert('提示','请选择要同步的表！','warning');
				return;
			}
			var tablesCodes = '',tablesNames='';
			for(var i = 0;i < row.length;i++)
			{
				tablesCodes += (i > 0?',':'') + row[i].Code;
				tablesNames += (i > 0?',':'') + row[i].Name;
			}
			var waitWin = $.messager.progress({  
				title:'请稍等',
				msg:'同步中...' 
			});  
			$.ajax({
				url: '/Column/SynchronousColumnBewrite',
				type: 'POST',
				data: {
					ServerName:ServerName,
					ServerUser:ServerUser,
					ServerPwd:ServerPwd,
					DatabaseName:databaseName,
					tablesCodes:tablesCodes,
					tablesNames:tablesNames
				},
				success: function (data) {
					if(data == ''){return;}
					var json = eval('(' + data + ')');
					if (!json.HasError) {
						$.messager.progress('bar').progressbar({
							onChange: function(value){
								if(value == 100){
									$.messager.progress('close'); 
									$('#dg-Tables').datagrid('unselectAll');
								}
							}
						});
					} else {
						$.messager.alert('提示',json.msg,'warning');
						$.messager.progress('close'); 
					}
				},
				error: function () {
					$.messager.alert('提示','服务器忙！','error');
					$.messager.progress('close'); 
				}
			});
		}
	}];
	//表grid
	$('#dg-Tables').datagrid({
		title:'表',
		url:'/Tables/GetPageTables',
		queryParams: {
			databaseCode:databaseCode,
			groupCode:groupCode,
			query:query
		},
		pageSize:pageSize,
		pagination: true,
		rownumbers: true,
		fitColumns: true,
		singleSelect: true,
		remoteSort:false,
		fit: true,
		border: false,            
		striped: true,
		toolbar:tablesToolbar,		
		columns:[[
			{field:'Name',title: '表名',width: 50,sortable:true},
			{field:'Alias',title: '别名',width: 50},
			{field:'Author',title: '作者',width: 50,hidden:true},
			{field:'CreateTime',title: '创建时间',width: 160,sortable:true,hidden:true}
		]],
		onClickRow:function(rowIndex, rowData){
			index = rowIndex;//得到选中行
			tablesCode = rowData.Code;
			tablesName = rowData.Name;
		},
		onDblClickRow:function(rowIndex, rowData){
			//加载字段grid
			$('#dg-Column').datagrid('load',{
				tablesCode:tablesCode,
				query:query
			});
			$("#dg-Column").datagrid({
				title:databaseName+' 数据库 | '+tablesName+" 表字段"
			});
		}
	});
	//表pager
	$('#dg-Tables').datagrid('getPager').pagination({
		pageSize: pageSize,
		pageList: [30,60,90,120,150],
		onBeforeRefresh:function(pageNumber, pageSize){
			$(this).pagination('loading');
			$(this).pagination('loaded');
		}
	});
	
	//字段window
	$('#window-Column').window({
		title:'列字段',
		width:345, 
		height:258,
		minimizable:false,
		maximizable:false,
		collapsible:false,
		modal:true,
		resizable:false,
		closed:true
	});
	//字段toolbar
	var columnToolbar = [{
		text:'创建新字段',
		iconCls:'icon-add',
		handler:function(){
			typeCmd = '1';
			$('#window-Column').window('open');
			$('#txtColumnName').val('');
			$('#txtColumnOwner').val('');
			$('#txtColumnType').combobox('setValue', '');
			$('#txtColumnBewrite').val('');
			//加载数据类型
			$('#txtColumnType').combobox('reload', '/Column/GetSQL2008ColumTypes');
		}
	},{
		text:'编辑字段',
		iconCls:'icon-edit',
		handler:function(){
			var row = $('#dg-Column').datagrid('getSelected');
			if(row == null){
				$.messager.alert('提示','请选择要编辑的字段信息！','warning');
				return;
			}
			typeCmd = '2';
			$('#window-Column').window('open');
			$('#txtColumnName').val(row.Name);
			$('#txtColumnOwner').val(row.Owner);
			$('#txtColumnType').combobox('setValue',row.Type);
			$('#txtColumnBewrite').val(row.Bewrite);
			//加载数据类型
			$('#txtColumnType').combobox('reload', '/Column/GetSQL2008ColumTypes');
		}
	},{
		text:'保存说明',
		iconCls:'icon-save',
		handler:function(){
			Port.SaveColumnRemark();
		}
	},{
		text:'移除字段',
		iconCls:'icon-remove',
		handler:function(){
			Port.DelColumn();
		}
	}];
	//字段grid
	$('#dg-Column').datagrid({
		title:'字段',
		url:'/Column/GetPageColumn',
		queryParams: {
			tablesCode:tablesCode,
			query:query
		},
		pageSize:pageSize,
		pagination: true,
		rownumbers: true,
		fitColumns: true,
		singleSelect: true,
		remoteSort:false,
		fit: true,
		border: false,            
		striped: true,
		toolbar:columnToolbar,		
		columns:[[
			{field:'Owner',title: '前缀',width: 40},
			{field:'Name',title: '列名',width: 70,sortable:true},
			{field:'Type',title: '类型',width: 80},
			{field:'Bewrite',title: '说明',width: 100,editor:'text'},
			{field:'Author',title: '作者',width: 50,hidden:true},
			{field:'CreateTime',title: '创建时间',width: 140,sortable:true,hidden:true}
		]],
		onClickRow:function(rowIndex, rowData){
			index = rowIndex;//得到选中行
			databaseCode = rowData.Code;
		},
		onDblClickRow:function(rowIndex, rowData){
			$('#dg-Column').datagrid('beginEdit', rowIndex);
			$('#dg-Column').datagrid('getEditors', rowIndex)[0].target.focus();
		}
	});
	//字段pager
	$('#dg-Column').datagrid('getPager').pagination({
		pageSize: pageSize,
		pageList: [30,60,90,120,150],
		onBeforeRefresh:function(pageNumber, pageSize){
			$(this).pagination('loading');
			$(this).pagination('loaded');
		}
	});
	
	//选项卡
	$('#tabs-Project').tabs({
		onSelect:function(title){
			switch(title)
			{
				case '项目':
					tablesCode='';//清空表编码
					databaseCode='';//清空数据库编码
					groupCode='';//清空功能分组编码
					$('#dg-Project').datagrid('unselectAll');
					$('#tabs-Project').tabs('disableTab', 1);	
					$('#tabs-Project').tabs('disableTab', 2);	
					$('#tabs-Project').tabs('disableTab', 3);
				break;
			}
		}   
	});
	//隐藏2、3、4选项卡
	$('#tabs-Project').tabs('disableTab', 1);	
	$('#tabs-Project').tabs('disableTab', 2);	
	$('#tabs-Project').tabs('disableTab', 3);
});
//调用方法接口
var Port = {
	SaveProject:function(){//保存项目信息
		Port.NoNull('#txtProjectName','请输入项目名称！');
		Port.NoNull('#txtProjectBewrite','请输入项目描述！');
		var parms;
		if(typeCmd == '1'){//新增
			//判断项目名称是否存在
			parms = { name: $("#txtProjectName").val(), bewrite: $("#txtProjectBewrite").val() };
		}
		if(typeCmd == '2'){//修改
			var row = $('#dg-Project').datagrid('getSelected');
			parms = { code: row.Code, name: $("#txtProjectName").val(), bewrite: $("#txtProjectBewrite").val() };
		}
		$.ajax({
			url: '/Project/SaveProject',
			type: 'POST',
			data: parms,
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					if(typeCmd == '2'){
						$('#dg-Project').datagrid('updateRow',{
							index: index,
							row: {
								Name:parms.name,
								Bewrite:parms.bewrite
							}
						});
					}else{
						$('#dg-Project').datagrid('load');
					}
					$("#window-Project").window("close");
				} else {
					$.messager.alert('提示',json.msg,'warning');
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	DelProject:function(){//删除项目信息
		var row = $('#dg-Project').datagrid('getSelected');
		if(row == null){
			$.messager.alert('提示','请选择要删除的项目！','warning');
			return;
		}
		$.messager.confirm('删除数据', '确实要永久性地删除选中数据？', function(r){
			if (r){
				$.ajax({
					url: '/Project/DeleteProject',
					type: 'POST',
					data: {code:row.Code},
					success: function (data) {
						if(data == ''){return;}
						var json = eval('(' + data + ')');
						if (!json.HasError) {
							$('#dg-Project').datagrid('deleteRow', index);
							$("#window-Project").window("close");
						} else {
							$.messager.alert('提示',json.msg,'warning');
						}
					},
					error: function () {
						$.messager.alert('提示','服务器忙！','error');
					}
				});
			}
		});
	},
	SaveDatabase:function(){//保存数据库
		Port.NoNull('#txtDatabaseName','请输入数据库名称！');
		Port.NoNull('#txtDatabaseAlias','请输入数据库别名！');
		Port.NoNull('#txtDatabaseBewrite','请输入数据库描述！');
		Port.NoNull('#txtDatabaseType','请输入数据库类型！');
		Port.NoNull('#txtServerName','请输入服务器名称！');
		Port.NoNull('#txtServerUser','请输入登录名！');
		Port.NoNull('#txtServerPwd','请输入密码！');
		var parms;
		if(typeCmd == '1'){//新增
			parms = {
				projectCode:projectCode,
				name: $("#txtDatabaseName").val(), 
				alias: $("#txtDatabaseAlias").val(),
				bewrite: $("#txtDatabaseBewrite").val(),
				type:$("#txtDatabaseType").val(),
				serverName:$("#txtServerName").val(),
				serverUser:$("#txtServerUser").val(),	
				serverPwd:$("#txtServerPwd").val()					
			};
		}
		if(typeCmd == '2'){//修改
			var row = $('#dg-Database').datagrid('getSelected');
			parms = { 
				code: row.Code, 
				projectCode:projectCode,
				name: $("#txtDatabaseName").val(), 
				alias: $("#txtDatabaseAlias").val(),
				bewrite: $("#txtDatabaseBewrite").val(),
				type:$("#txtDatabaseType").val(),
				serverName:$("#txtServerName").val(),
				serverUser:$("#txtServerUser").val(),	
				serverPwd:$("#txtServerPwd").val()
			};
		}
		$.ajax({
			url: '/Database/SaveDatabase',
			type: 'POST',
			data: parms,
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					if(typeCmd == '2'){
						$('#dg-Database').datagrid('updateRow',{
							index: index,
							row: {
								Name:parms.name,
								Alias:parms.alias,
								Type:parms.type,
								ServerName:parms.serverName,
								Bewrite:parms.bewrite
							}
						});
					}else{
						$('#dg-Database').datagrid('load');
					}
					$("#window-Database").window("close");
				} else {
					$.messager.alert('提示',json.msg,'warning');
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	DelDatabase:function(){//删除数据库
		var row = $('#dg-Database').datagrid('getSelected');
		if(row == null){
			$.messager.alert('提示','请选择要删除的数据库！','warning');
			return;
		}
		$.messager.confirm('删除数据', '确实要永久性地删除选中数据？', function(r){
			if (r){
				$.ajax({
					url: '/Database/DeleteDatabase',
					type: 'POST',
					data: {code:row.Code},
					success: function (data) {
						if(data == ''){return;}
						var json = eval('(' + data + ')');
						if (!json.HasError) {
							$('#dg-Database').datagrid('deleteRow', index);
							$("#window-Database").window("close");
						} else {
							$.messager.alert('提示',json.msg,'warning');
						}
					},
					error: function () {
						$.messager.alert('提示','服务器忙！','error');
					}
				});
			}
		});
	},
	SaveGroup:function(){//保存分组
		Port.NoNull('#txtGroupName','请输入功能分组名称！');
		Port.NoNull('#txtGroupBewrite','请输入功能分组描述！');
		if(projectCode == ''){
			$.messager.alert('提示','请选择项目！','warning');
			$("#txtGroupBewrite").focus();
			return;			
		}
		var parms;
		if(typeCmd == '1'){//新增
			parms = { name: $("#txtGroupName").val(), bewrite: $("#txtGroupBewrite").val(), projectCode: projectCode};
		}
		if(typeCmd == '2'){//修改
			var row = $('#dg-Group').datagrid('getSelected');
			parms = { code: row.Code, name: $("#txtGroupName").val(), bewrite: $("#txtGroupBewrite").val(), projectCode: projectCode};
		}
		$.ajax({
			url: '/Group/SaveGroup',
			type: 'POST',
			data: parms,
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					if(typeCmd == '2'){
						$('#dg-Group').datagrid('updateRow',{
							index: index,
							row: {
								Name:parms.name,
								Bewrite:parms.bewrite
							}
						});
					}else{
						$('#dg-Group').datagrid('load');
					}
					$("#window-Group").window("close");
				} else {
					$.messager.alert('提示',json.msg,'warning');
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	DelGroup:function(){//删除分组
		var row = $('#dg-Group').datagrid('getSelected');
		if(row == null){
			$.messager.alert('提示','请选择要删除的功能分组！','warning');
			return;
		}
		$.messager.confirm('删除数据', '确实要永久性地删除选中数据？', function(r){
			if (r){
				$.ajax({
					url: '/Group/DeleteGroup',
					type: 'POST',
					data: {code:row.Code},
					success: function (data) {
						if(data == ''){return;}
						var json = eval('(' + data + ')');
						if (!json.HasError) {
							$('#dg-Group').datagrid('deleteRow', index);
							$("#window-Group").window("close");
						} else {
							$.messager.alert('提示',json.msg,'warning');
						}
					},
					error: function () {
						$.messager.alert('提示','服务器忙！','error');
					}
				});
			}
		});
	},
	SaveTable:function(){//保存表
		Port.NoNull('#txtTablesName','请输入表名称！');
		Port.NoNull('#txtTablesAlias','请输入表别名！');
		if(databaseCode == ''){
			$.messager.alert('提示','请选择数据库！','warning');
			return;			
		}
		if(groupCode == ''){
			$.messager.alert('提示','请选择功能分组！','warning');
			return;			
		}
		var parms;
		if(typeCmd == '1'){//新增
			parms = { name: $("#txtTablesName").val(), alias: $("#txtTablesAlias").val(), databaseCode: databaseCode, groupCode:groupCode};
		}
		if(typeCmd == '2'){//修改
			var row = $('#dg-Tables').datagrid('getSelected');
			parms = { code: row.Code, name: $("#txtTablesName").val(), alias: $("#txtTablesAlias").val(), databaseCode: databaseCode, groupCode:groupCode};
		}
		$.ajax({
			url: '/Tables/SaveTables',
			type: 'POST',
			data: parms,
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					if(typeCmd == '2'){
						$('#dg-Tables').datagrid('updateRow',{
							index: index,
							row: {
								Name:parms.name,
								Alias:parms.alias
							}
						});
					}else{
						$('#dg-Tables').datagrid('load');
					}
					$("#window-Tables").window("close");
				} else {
					$.messager.alert('提示',json.msg,'warning'); 
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	DelTable:function(){//删除表
		var row = $('#dg-Tables').datagrid('getSelections');
		if(row.length == 0){
			$.messager.alert('提示','请选择要删除的表！','warning');
			return;
		}
		$.messager.confirm('删除数据', '确实要永久性地删除选中数据？', function(r){
			if (r){
				var codes = '';
				for(var i = 0;i < row.length;i++)
				{
					codes += (i > 0?',':'') + row[i].Code;
				}
				$.ajax({
					url: '/Tables/DeleteTables',
					type: 'POST',
					data: {codes:codes},
					success: function (data) {
						if(data == ''){return;}
						var json = eval('(' + data + ')');
						if (!json.HasError) {
							for(var i = 0; i < row.length; i++){
								var index = $('#dg-Tables').datagrid('getRowIndex',row[i]);
								$('#dg-Tables').datagrid('deleteRow', index);
							}
							$('#dg-Column').datagrid('load');
							$("#dg-Column").datagrid({
								title:'表字段'
							});
						} else {
							$.messager.alert('提示',json.msg,'warning'); 
						}
					},
					error: function () {
						$.messager.alert('提示','服务器忙！','error');
					}
				});
			}
		});
	},
	ImportTable:function(){//导入表
	    var row = $('#Import-Grid').datagrid('getSelections');
		if(row.length == 0){
			$.messager.alert('提示','请选中需要导入的表！','warning');
			return;
		}
		if(groupCode == ''){
			$.messager.alert('提示','请选择功能分组！','warning');
			return;			
		}
		//开始导入表
		var tables = '';
		for(var i = 0;i < row.length;i++){
			tables += (i > 0?',':'') + row[i].name;
		}
		$.ajax({
			url: '/Tables/ImportTables',
			type: 'POST',
			data: {
				tables:tables,
				ServerName:ServerName,
				ServerUser:ServerUser,
				ServerPwd:ServerPwd,
				DatabaseName:databaseName,
				databaseCode:databaseCode,
				groupCode:groupCode
			},
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					$('#dg-Tables').datagrid('load');
					$('#dg-Column').datagrid('load');
					//循环删除导入表的选中行
					for(var i = 0; i < row.length; i++){
						var index = $('#Import-Grid').datagrid('getRowIndex',row[i]);
						$('#Import-Grid').datagrid('deleteRow',index);
					}
					if($("#Import-Grid").datagrid("getRows").length == 0)
					{
						$("#window-Import-Table").window("close");
					}
				} else {
					$.messager.alert('提示',json.msg,'warning');
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	ImportTableRefresh:function(){//刷新导入表
		$('#Import-Grid').datagrid('load');
	},
	ImportTableAll:function(btn){//判断浏览器
		if($.browser.msie) {
			Port.ImportBtnSelete(btn,btn.innerText,$('#Import-Grid'));
		}else{
			Port.ImportBtnSelete(btn,btn.textContent,$('#Import-Grid'));
		}
	},
	ImportBtnSelete:function(btn,text,grid){//导入按钮全选或撤销
		if(text == '全选')
		{
			grid.datagrid('selectAll');
			btn.innerHTML = '<span class="l-btn-left"><span class="l-btn-text icon-config l-btn-icon-left">撤销</span></span>';
		}else{
			grid.datagrid('unselectAll');
			btn.innerHTML = '<span class="l-btn-left"><span class="l-btn-text icon-config l-btn-icon-left">全选</span></span>';
		}
	},
	SaveColumn:function(){//保存表字段
		Port.NoNull('#txtColumnName','请输入字段列名！');
		Port.NoNull('#txtColumnOwner','请输入字段前缀！');
		if($('#txtColumnType').combo('getText') == ''){
			$.messager.alert('提示','请选择字段类型！','warning');
			$('#txtColumnType').focus();
			return;			
		}
		if(tablesCode == ''){
			$.messager.alert('提示','请选择相应的表！','warning');
			return;			
		}
		var parms;
		if(typeCmd == '1'){//新增
			parms = {
				tablesCode: tablesCode,
				owner:$("#txtColumnOwner").val(),
				name: $("#txtColumnName").val(),
				type: $('#txtColumnType').combo('getText'), 
				bewrite: $("#txtColumnBewrite").val(), 
				fieldState: 1,
				hideAuthor: ''
			};
		}
		if(typeCmd == '2'){//修改
			var row = $('#dg-Column').datagrid('getSelected');
			parms = {
				code: row.Code,
				tablesCode: tablesCode,
				owner:$("#txtColumnOwner").val(), 
				name: $("#txtColumnName").val(),
				type: $('#txtColumnType').combo('getText'),
				bewrite: $("#txtColumnBewrite").val(), 
				fieldState: 1,
				hideAuthor: ''
			};
		}
		$.ajax({
			url: '/Column/SaveColumn',
			type: 'POST',
			data: parms,
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					if(typeCmd == '2'){
						$('#dg-Column').datagrid('updateRow',{
							index: index,
							row: {
								Owner:parms.owner,
								Name:parms.name,
								Type:parms.type,
								Bewrite:parms.bewrite
							}
						});
					}else{
						$('#dg-Column').datagrid('load');
					}
					$("#window-Column").window("close");
				} else {
					$.messager.alert('提示',json.msg,'warning');
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	SaveColumnRemark:function(){//保存字段说明
		//首先结束编辑行
		var rows = $('#dg-Column').datagrid('getRows');
		for (var i=0;i<rows.length;i++) {  
			$('#dg-Column').datagrid('endEdit', i);
		}
		//获取改变的行
		var changesRows = $('#dg-Column').datagrid('getChanges');
		if(changesRows.length < 1)
		{
			$.messager.alert('提示','请改变或填写相应的字段说明！','warning');
			return;
		}
		var parms = [];
		for(var j=0;j<changesRows.length;j++){
			parms.push({Code:changesRows[j].Code,Bewrite:changesRows[j].Bewrite});
		}
		var waitWin = $.messager.progress({  
			title:'请稍等',
			msg:'保存中...' 
		});  
		//批量保存
		$.ajax({
			url: '/Column/SaveColumnRemark',
			type: 'POST',
			data: {changRecord:JSON.stringify(parms)},
			success: function (data) {
				if(data == ''){return;}
				var json = eval('(' + data + ')');
				if (!json.HasError) {
					$.messager.progress('bar').progressbar({
						onChange: function(value){
							if(value == 100){
								$.messager.progress('close'); 
							}
						}
					});
				} else {
					$.messager.alert('提示',json.msg,'warning');
				}
			},
			error: function () {
				$.messager.alert('提示','服务器忙！','error');
			}
		});
	},
	DelColumn:function(){//删除表字段
		var row = $('#dg-Column').datagrid('getSelected');
		if(row == null){
			$.messager.alert('提示','请选择要删除的字段！','warning');
			return;
		}
		$.messager.confirm('删除数据', '确实要永久性地删除选中数据？', function(r){
			if (r){
				$.ajax({
					url: '/Column/DeleteColumn',
					type: 'POST',
					data: {code:row.Code},
					success: function (data) {
						if(data == ''){return;}
						var json = eval('(' + data + ')');
						if (!json.HasError) {
							$('#dg-Column').datagrid('deleteRow', index);
						} else {
							$.messager.alert('提示',json.msg,'warning');
						}
					},
					error: function () {
						$.messager.alert('提示','服务器忙！','error');
					}
				});
			}
		});
	},
	NoNull:function(Id,Msg){//非空判断
		if($(Id).val() == ''){
			$.messager.alert('提示',Msg,'warning');
			$(Id).focus();
			return;			
		}
	},
	TableIcon:function(value){//表图标
		return '<img src="../Content/Images/grid.png" border="0" align="absmiddle"> '+value;
	}
};