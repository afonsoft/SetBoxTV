!function(e,t){void 0===e&&void 0!==window&&(e=window),"function"==typeof define&&define.amd?define(["jquery"],function(e){return t(e)}):"object"==typeof module&&module.exports?module.exports=t(require("jquery")):t(e.jQuery)}(this,function(e){e.fn.selectpicker.defaults={noneSelectedText:"لم يتم إختيار شئ",noneResultsText:"لا توجد نتائج مطابقة لـ {0}",countSelectedText:function(e,t){return 1==e?"{0} خيار تم إختياره":"{0} خيارات تمت إختيارها"},maxOptionsText:function(e,t){return[1==e?"تخطى الحد المسموح ({n} خيار بحد أقصى)":"تخطى الحد المسموح ({n} خيارات بحد أقصى)",1==t?"تخطى الحد المسموح للمجموعة ({n} خيار بحد أقصى)":"تخطى الحد المسموح للمجموعة ({n} خيارات بحد أقصى)"]},selectAllText:"إختيار الجميع",deselectAllText:"إلغاء إختيار الجميع",multipleSeparator:"، "}});