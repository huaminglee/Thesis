var Ax = {
    SetTheme: function (resource, name, value) {
        Ext.net.ResourceMgr.setTheme(resource, name);
        Ax.SetCookie('Theme', value, 365 * 10);
    },
    ShowMessage: function (title, message, buttons, icon) {
        Ext.Msg.show({
            title: title,
            msg: message,
            buttons: buttons,
            icon: icon
        });
    },
    ShowNotification: function (title, message, iconCls) {
        Ext.net.Notification.show({
            iconCls: iconCls,
            pinEvent: 'click',
            hideDelay: 2000,
            html: message,
            title: title
        });
    },
    Wait: function (title, message) {
        Ext.Msg.wait(message, title);
    },
    IsValidComponent: function (com) {
        var a = true;
        a = Ax.IsValidControl(com, a);
        return a;
    },
    IsValidForm: function (form) {
        var a = true;
        form.getForm().items.each(function (b) { if (!Ax.IsValidControl(b, a)) { a = false; } });
        return a;
    },
    IsValidControl: function (com, a) {
        var containerItems = new Array("panel", "compositefield");
        if (containerItems.indexOf(com.xtype) != -1) { com.items.each(function (c) { if (!Ax.IsValidControl(c, a)) { a = false; } }); }
        else { if (com.validate != undefined && !com.validate()) { if (a) { com.focus(); } a = false; } }
        return a;
    },
    GetFileUploadIdsByForm: function (form) {
        var Ids = '';
        form.items.each(function (b) { Ids = Ax.GetFileUploadIds(b, Ids); });
        return Ids;
    },
    GetFileUploadIds: function (com, ids) {
        var containerItems = new Array("panel", "compositefield");
        if (containerItems.indexOf(com.xtype) != -1) { com.items.each(function (c) { ids = Ax.GetFileUploadIds(c, ids); }); }
        else { if (com.xtype == 'fileuploadfield') ids = ids + com.id + ','; }
        return ids;
    },
    Mask: function (message) {
        Ext.getBody().mask(message, 'x-mask-loading');
    },
    GetSelectedRowIndex: function (item) {
        var selections = item.selModel.selections;
        return selections.length == 1 ?
                item.selectedIds[selections.keys[0]].index : -1;
    },
    HashCode: function (str) {
        var hash = 1315423911;

        for (var i = 0; i < str.length; i++) {
            hash ^= ((hash << 5) + str.charCodeAt(i) + (hash >> 2));
        }

        return (hash & 0x7FFFFFFF);
    },
    AddTab: function (config) {
        if (Ext.isEmpty(config.url, false)) {
            return;
        }

        var tp = Ext.getCmp(config.tabName);
        var id = this.HashCode(config.url);
        var tab = tp.getComponent(id);

        if (!tab) {
            tab = tp.addTab({
                id: id.toString(),
                title: Ax.FormatTitle(config.title, 24),
                iconCls: config.icon || 'icon-applicationdouble',
                closable: true,
                autoLoad: {
                    showMask: true,
                    url: config.url,
                    mode: 'iframe',
                    noCache: true,
                    maskMsg: "Loading '" + Ax.SubString(config.title, 30) + "'...",
                    scripts: true,
                    passParentSize: config.passParentSize
                }
            });
        } else {
            tp.setActiveTab(tab);
            Ext.get(tab.tabEl).frame();
        }
    },
    AddDetailTab: function (config) {
        Ax.AddDetailTab(config, 0, null);
    },
    AddDetailTab: function (config, rowIndex, item) {
        if (rowIndex < 0 || Ext.isEmpty(config.url, false))
            return;

        var tp = Ext.getCmp(config.tabName);
        var id = '';

        if (item != null) {
            var row = item.store.data.items[rowIndex];
            config.url = Ax.QueryFormat(config.url, '?', '/' + row.id);
            eval("config.title = row.data." + config.title);
            id = this.HashCode(config.url);
        }
        else {
            config.url = Ax.QueryFormat(config.url, '?', '/0');
            id = Ax.NewId(config.url);
        }

        Ax.OpenTab(tp, id, config);
    },
    CloseTab: function (win, gridPanelID) {
        if (!Ax.IsNullOrEmpty(gridPanelID)) eval("win.parent." + gridPanelID + ".store.reload();");
        if (win.parentAutoLoadControl) {
            if (win.parentAutoLoadControl.isDestroyable == undefined || win.parentAutoLoadControl.isDestroyable == true)
                win.parentAutoLoadControl.destroy();
            else
                win.parentAutoLoadControl.hide();
        }
    },
    AddNewTab: function (win, config) {
        if (!Ax.IsNullOrEmpty(config.gridPanelID))
            eval("win.parent." + config.gridPanelID + ".store.reload();");

        if (win.parentAutoLoadControl.id.indexOf('_Window') == -1 && win.parentAutoLoadControl.xtype && win.parentAutoLoadControl.xtype != 'window') {
            var id = win.name.replace('_IFrame', '');
            config.url = Ax.QueryFormat(config.url, '?', '/0');
            eval("var tp = win.parent." + win.parent[id].tabEl.id.replace('__' + id, ''));
            var newId = Ax.NewId(config.url);
            var tab = tp.getComponent(newId);

            Ax.OpenTab(tp, newId, config);
            Ax.CloseTab(win, config.gridPanelID);
        }
        else {
            Ax.ChangeTitle(win, config.title);
            win.location = Ax.QueryFormat(config.url, '?', '/0') + win.location.search;
        }
    },
    ChangeTitle: function (win, title) {
        win.parentAutoLoadControl.setTitle(title);
    },
    OpenTab: function (tp, id, config) {
        var tab = tp.getComponent(id);
        if (!tab) {
            tab = tp.addTab({
                id: id.toString(),
                title: Ax.FormatTitle(config.title, 24),
                closable: true,
                listeners: {
                    close: function () {
                        if (!Ax.IsNullOrEmpty(config.gridPanelID)) {
                            if (Ext.isIE)
                                eval('tp.el.dom.document.parentWindow.' + config.gridPanelID + '.store.reload();');
                            else
                                eval('tp.el.dom.parentNode.ownerDocument.defaultView.' + config.gridPanelID + '.store.reload();');
                        }
                    }
                },
                autoLoad: {
                    showMask: true,
                    url: config.url,
                    mode: 'iframe',
                    noCache: true,
                    maskMsg: "Loading '" + Ax.SubString(config.title, 30) + "'...",
                    scripts: true,
                    passParentSize: config.passParentSize,
                    params: { gridPanelID: config.gridPanelID }
                }
            });
        } else {
            tp.setActiveTab(tab);
            Ext.get(tab.tabEl).frame();
        }
    },
    AddWindow: function (config) {
        Ax.AddWindow(config, 0, null);
    },
    AddWindow: function (config, rowIndex, item) {
        if (rowIndex < 0 || Ext.isEmpty(config.url, false))
            return;

        var id = '';

        if (item != null) {
            var row = item.store.data.items[rowIndex];
            config.url = Ax.QueryFormat(config.url, '?', '/' + row.id);
            eval("config.title = row.data." + config.title);
            id = this.HashCode(config.url);
        }
        else {
            config.url = Ax.QueryFormat(config.url, '?', '/0');
            id = Ax.NewId(config.url);
        }

        var win = new Ext.Window({
            id: id.toString() + '_Window',
            title: Ax.FormatTitle(config.title, 24),
            maximizable: false,
            minimizable: false,
            maximized: true,
            modal: true,
            listeners: {
                hide: function () {
                    if (!Ax.IsNullOrEmpty(config.gridPanelID)) {
                        if (Ext.isIE)
                            eval('win.el.dom.document.parentWindow.' + config.gridPanelID + '.store.reload();');
                        else
                            eval('win.el.dom.parentNode.ownerDocument.defaultView.' + config.gridPanelID + '.store.reload();');
                    }
                    win.destroy();
                }
            },
            autoLoad: {
                showMask: true,
                url: config.url,
                mode: "iframe",
                maskMsg: "Loading '" + Ax.SubString(config.title, 30) + "'...",
                params: { gridPanelID: config.gridPanelID }
            }
        });
        win.show();
    },
    LoadWindow: function (config, win) {
        Ax.AddWindow(config, win, 0, null);
    },
    LoadWindow: function (config, win, rowIndex, item) {
        if (rowIndex < 0 || Ext.isEmpty(config.url, false))
            return;

        var id = '';

        if (item != null) {
            var row = item.store.data.items[rowIndex];
            config.url = Ax.QueryFormat(config.url, '?', '/' + row.id);
            eval("config.title = row.data." + config.title);
            id = this.HashCode(config.url);
        }
        else {
            config.url = Ax.QueryFormat(config.url, '?', '/0');
            id = Ax.NewId(config.url);
        }

        win.show();
        win.autoLoad.url = config.url;
        win.setTitle(Ax.FormatTitle(config.title, 24));
        win.maskMsg = 'Loading ' + Ax.FormatTitle(config.title, 24);
        win.reload();
    },
    SelectedRecords: function (grdPnl) {
        return grdPnl.selModel.selections.keys;
    },
    SetComboValue: function (combo, listItem) {
        if (combo) {
            combo.hiddenField.value = listItem.value;
            combo.setRawValue(listItem.text);
            combo.lastSelectionText = listItem.text;
            combo.value = listItem.value;

            if (combo.triggers)
                combo.triggers[0][combo.getRawValue().toString().length == 0 ? 'hide' : 'show']();
        }
    },
    GetCookie: function (name) {
        if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf(name + "=");
            if (c_start != -1) {
                c_start = c_start + name.length + 1;
                c_end = document.cookie.indexOf(";", c_start);
                if (c_end == -1) c_end = document.cookie.length;
                return unescape(document.cookie.substring(c_start, c_end));
            }
        }
        return "";
    },
    SetCookie: function (name, value, expiredays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + expiredays);
        document.cookie = name + "=" + escape(value) +
        ((expiredays == null) ? "" : ";expires=" + exdate.toUTCString());
    },
    NewId: function (url) {
        return this.HashCode(url).toString() + Math.floor(Math.random() * -2000);
    },
    IsNullOrEmpty: function (item) {
        return !(item && item.toString().length > 0);
    },
    QueryFormat: function (text, oldValue, newValue) {
        return text.indexOf(oldValue) == -1 ? text + newValue : text.replace(oldValue, newValue + oldValue);
    },
    DisableDecimals: function (numberField, allowDecimals) {
        numberField.allowDecimals = allowDecimals;
        var allowed = numberField.baseChars + '';
        if (numberField.allowDecimals) {
            allowed += numberField.decimalSeparator;
        }
        if (numberField.allowNegative) {
            allowed += '-';
        }
        allowed = Ext.escapeRe(allowed);
        numberField.maskRe = new RegExp('[' + allowed + ']');
    },
    UUID: function () {
        var chars = '0123456789abcdef'.split('');

        var uuid = [], rnd = Math.random, r;
        uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
        uuid[14] = '4'; // version 4

        for (var i = 0; i < 36; i++) {
            if (!uuid[i]) {
                r = 0 | rnd() * 16;

                uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r & 0xf];
            }
        }

        return uuid.join('');
    },
    FormatFilterText: function (text) {
        return "'" + text + "'";
    },
    FilterItem: function (id, propertyName, whereCondition, filterCondition, startValue, endValue) {
        debugger;
        var filterObj = '{';
        filterObj += '"Id":"' + id + '",';
        filterObj += '"PropertyName":"' + propertyName + '",';
        filterObj += '"WhereCondition":"' + whereCondition + '",';
        filterObj += '"FilterCondition":"' + filterCondition + '",';
        filterObj += '"StartValue":"' + startValue + '",';
        filterObj += '"EndValue":"' + endValue + '"';
        filterObj += '}';
        return filterObj;
    },
    Replace: function (v, oldValue, newValue) {
        while (v.indexOf(oldValue) != -1) { v = v.replace(oldValue, newValue); } return v;
    },
    ClearDirtyForm: function (form) {
        var v = Ax.Replace(Ext.encode(form.getForm().getValues()), '_SelIndex":"-1"', '_SelIndex":""');
        v = Ax.Replace(v, '_ActivePage":""', '_ActivePage":"1"');
        v = Ax.Replace(v, '_Collapsed":"true"', '_Collapsed":""');
        v = Ax.Replace(v, '_Collapsed":"false"', '_Collapsed":""');
        return v;
    },
    CheckRequired: function (combo) {
        var _value = combo.getValue();
        if (Ax.IsNullOrEmpty(_value)) {
            combo.markInvalid('This field is required.');
            combo.focus();
            return false;
        }
        return true;
    },
    CheckDuplicate: function (gridPanel, property, warningProperty, combo, comboProperty) {
        var items = gridPanel.store.data.items;
        var value = eval('combo.store.data.items[combo.getSelectedIndex()].json.' + comboProperty);
        for (var i = 0; i < items.length; i++) {
            if (eval('items[i].data.' + property) == value) {
                combo.markInvalid(eval('items[i].data.' + warningProperty) + ' is exist.');
                return false;
            }
        }
        return true;
    },
    LoadValuesFromRecords: function (records, property, hiddenObj) {
        if (records && records.length == 0) { hiddenObj.setValue(null); return; }
        var _values = '';
        for (var i = 0; i < records.length; i++)
            _values += eval('records[i].data.' + property) + ',';
        _values = _values.substring(0, _values.length - 1);
        hiddenObj.setValue(_values);
    },
    AddValueFromComboBox: function (combo, comboProperty, hiddenObj) {
        var _values = hiddenObj.getValue();
        hiddenObj.setValue((_values.length > 0 ? _values + ',' : '') + eval('combo.store.data.items[combo.getSelectedIndex()].json.' + comboProperty));
    },
    DeleteValuesFromGridPanel: function (gridPanel, property, hiddenObj, buttonDelete) {
        var selectedItems = gridPanel.selModel.selections.items;
        if (selectedItems.length == 0) return;
        var _values = hiddenObj.getValue();
        while (selectedItems.length > 0) {
            if (_values && _values != null) {
                var _value = eval('selectedItems[0].data.' + property);
                if (_values.indexOf(_value + ',') != -1)
                    _values = _values.replace(_value + ',', '');
                else {
                    _values = _values.replace(_value, '');
                    if (_values.length > 0 && _values.substring(_values.length - 1) == ',')
                        _values = _values.substring(0, _values.length - 1);
                }
            }
            gridPanel.store.remove(selectedItems[0]);
        }
        hiddenObj.setValue(_values);

        if (gridPanel.store.data.items.length == 0)
            buttonDelete.disable();
    },
    ClearComboBoxWithTrigger: function (combo) {
        combo.clearValue();
        combo.triggers[0].hide();
    },
    SubString: function (v, length) {
        return v != undefined && v != null && v.length > length ? v.substring(0, length) : v;
    },
    FormatTitle: function (v, length) {
        return v != undefined && v != null && v.length > length ? v.substring(0, length) + '...' : v;
    },
    FilterObj: function (item) {
        return Ax.FilterItem(item.Id, item.PropertyName, item.WhereCondition, item.FilterCondition, item.StartValue, item.EndValue);
    },
    EUMoney: function (v) {
        if (Ax.IsNullOrEmpty(v)) return v;
        var val = v.toString();
        while (val.indexOf('.') != -1) { val = val.replace(/\./, '') }
        val = val.replace(/[€]/g, '').replace(/\,/, '.');
        return Ax.MoneyFormat(true, val, '.', ',', '€', false);
    },
    UsMoney: function (v) {
        if (Ax.IsNullOrEmpty(v)) return v;
        var val = v.toString();
        while (val.indexOf(',') != -1) { val = val.replace(/\,/, '') }
        val = val.replace(/[$]/g, '');
        return Ax.MoneyFormat(false, val, ',', '.', '$', true);
    },
    MoneyFormat: function (hasSpace, v, dSeperator, tSeperator, symbol, isLeft) {
        v = (Math.round((v - 0) * 100)) / 100;
        v = (v == Math.floor(v)) ? v + ".00" : ((v * 10 == Math.floor(v * 10)) ? v + "0" : v);
        v = String(v);
        var ps = v.split('.'),
                whole = ps[0],
                sub = ps[1] ? tSeperator + ps[1] : tSeperator + '00',
                r = /(\d+)(\d{3})/;
        while (r.test(whole)) {
            whole = whole.replace(r, '$1' + dSeperator + '$2');
        }
        v = whole + sub;
        if (v.charAt(0) == '-') {
            return '-' + isLeft ? symbol + (hasSpace ? ' ' : '') + v.substr(1) : v.substr(1) + (hasSpace ? ' ' : '') + symbol;
        }
        return isLeft ? symbol + (hasSpace ? ' ' : '') + v : v + (hasSpace ? ' ' : '') + symbol;
    }
}


