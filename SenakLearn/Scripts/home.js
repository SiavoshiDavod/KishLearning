function countPreviewItemByClient(objType, objId) {
    //debugger;
    console.log(objType);
    var url = '/Home/ObjCount';
    $.ajax({
        url: url,
        data: { ObjType: objType, ObjId: objId },
        type: 'post',
        async: true
        //success: function (res) {

        //}
    });
}