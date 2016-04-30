$("select[name=ShippingState]").change(function(e) {
	var id = $(e.target).parent().parent().attr("data-id");
	var state = $(e.target).val();
	var arr = { id: id, state: state};
	var lookup = {
		"Pending": 0,
		"Canceled": 1,
		"Delivered": 2
	};
	var headers = {};
	headers['__RequestVerificationToken'] = $('input[name="__RequestVerificationToken"]').val();
            
	$(e.target).parent().parent().hide();
	$.ajax({
		url: url,
		type: "POST",
		headers: headers,
		data: JSON.stringify(arr),
		contentType: "application/json; charset=utf-8",
		success: function (response) {
			if (state == lookup["Pending"]) {
				$("#pending").text(parseInt($("#pending").text()) + 1);
			} else if (state == lookup["Delivered"]) {
				$("#delivered").text(parseInt($("#delivered").text()) + 1);
			} else if (state == lookup["Canceled"]) {
				$("#canceled").text(parseInt($("#canceled").text()) + 1);
			}
			if (tab == "Pending") {
				$("#pending").text(parseInt($("#pending").text()) - 1);
			} else if (tab == "Delivered") {
				$("#delivered").text(parseInt($("#delivered").text()) - 1);
			} else if (tab == "Canceled") {
				$("#canceled").text(parseInt($("#canceled").text()) - 1);
			}
			$("#n").text(parseInt($("#n").text()) - 1);
			if (parseInt($("#n").text()) < 2) {
				$("#be").text("is");
			}
			if (parseInt($("#n").text()) == 0) {
				$("#n").text("no");
			}
		}
	});
});