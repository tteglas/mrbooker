/*
@license
dhtmlxScheduler v.5.0.0 Stardard

This software is covered by GPL license. You also can obtain Commercial or Enterprise license to use it in non-GPL project - please contact sales@dhtmlx.com. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
scheduler.__recurring_template='<div class="dhx_form_repeat" dir="rtl"> <form> <div class="dhx_repeat_left"> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="day" />יומי</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="week"/>שבועי</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="month" checked />חודשי</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="repeat" value="year" />שנתי</label> </div> <div class="dhx_repeat_divider"></div> <div class="dhx_repeat_center"> <div style="display:none;" id="dhx_repeat_day"> <label><input class="dhx_repeat_radio" type="radio" name="day_type" value="d"/>חזור כל</label><input class="dhx_repeat_text" type="text" name="day_count" value="1" />ימים<br /> <label><input class="dhx_repeat_radio" type="radio" name="day_type" checked value="w"/>חזור כל יום עבודה</label> </div> <div style="display:none;" id="dhx_repeat_week"> חזור כל<input class="dhx_repeat_text" type="text" name="week_count" value="1" />שבוע לפי ימים:<br /> <table class="dhx_repeat_days"> <tr> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="1" />שני</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="4" />חמישי</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="2" />שלישיlabel><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="5" />שישי</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="3" />רביעי</label><br /> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="6" />שבת</label> </td> <td> <label><input class="dhx_repeat_checkbox" type="checkbox" name="week_day" value="0" />ראשון</label><br /><br /> </td> </tr> </table> </div> <div id="dhx_repeat_month"> <label><input class="dhx_repeat_radio" type="radio" name="month_type" value="d"/>חזור כל</label><input class="dhx_repeat_text" type="text" name="month_day" value="1" />ימים כל<input class="dhx_repeat_text" type="text" name="month_count" value="1" />חודשים<br /> <label><input class="dhx_repeat_radio" type="radio" name="month_type" checked value="w"/>כל</label><input class="dhx_repeat_text" type="text" name="month_week2" value="1" /><select name="month_day2"><option value="1" selected >שני<option value="2">שלישי<option value="3">רביעי<option value="4">חמישי<option value="5">שישי<option value="6">שבת<option value="0">ראשון</select>חזור כל<input class="dhx_repeat_text" type="text" name="month_count2" value="1" />חודש<br /> </div> <div style="display:none;" id="dhx_repeat_year"> <label><input class="dhx_repeat_radio" type="radio" name="year_type" value="d"/>כל</label><input class="dhx_repeat_text" type="text" name="year_day" value="1" />ימים<select name="year_month"><option value="0" selected >ינואר<option value="1">פברואר<option value="2">מרץ<option value="3">אפריל<option value="4">מאי<option value="5">יוני<option value="6">יולי<option value="7">אוגוסט<option value="8">ספטמבר<option value="9">אוקטובר<option value="10">נובמבר<option value="11">דצמבר</select>חודש<br /> <label><input class="dhx_repeat_radio" type="radio" name="year_type" checked value="w"/>כל</label><input class="dhx_repeat_text" type="text" name="year_week2" value="1" /><select name="year_day2"><option value="7">ראשון<option value="1" selected >שני<option value="2">שלישי<option value="3">רביעי<option value="4">חמישי<option value="5">שישי<option value="6">שבת</select>בחודש<select name="year_month2"><option value="0" selected >ינואר<option value="1">פברואר<option value="2">מרץ<option value="3">אפריל<option value="4">מאי<option value="5">יוני<option value="6">יולי<option value="7">אוגוסט<option value="8">ספטמבר<option value="9">אוקטובר<option value="10">נובמבר<option value="11">דצמבר</select><br /> </div> </div> <div class="dhx_repeat_divider"></div> <div class="dhx_repeat_right"> <label><input class="dhx_repeat_radio" type="radio" name="end" checked/>לעולם לא מסתיים</label><br /> <label><input class="dhx_repeat_radio" type="radio" name="end" />אחרי</label><input class="dhx_repeat_text" type="text" name="occurences_count" value="1" />אירועים<br /> <label><input class="dhx_repeat_radio" type="radio" name="end" />מסתיים ב</label><input class="dhx_repeat_date" type="text" name="date_of_end" value="'+scheduler.config.repeat_date_of_end+'" /><br /> </div> </form> </div> <div style="clear:both"> </div>';

