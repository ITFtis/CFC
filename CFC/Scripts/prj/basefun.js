//過濾縣市的鄉鎮清單
function FilterCityTown(city, oTown) {

    $(oTown).find('option').each(function (i) {

        //下拉：請選擇
        if ($(this).val() == '')
            return; // 等於continue

        //下拉：鄉鎮條件
        var citycode = $(this).attr('data-citycode');
        if (citycode != city) {
            $(this).css('display', 'none');
        } else {
            $(this).css('display', 'inline-block');
        }
    });

    //預設鄉鎮選項
    $(oTown).val('');
}

//四捨五入，小數位數
function MathRound(number, decimalPlaces) {
    const factor = 10 ** decimalPlaces;
    return Math.round(number * factor) / factor;
}