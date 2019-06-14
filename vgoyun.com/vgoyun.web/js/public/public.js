$(document).ready(function(){
//	导航
$(".head-nav > ul > li").hover(function(){
	$(this).addClass("on");
	$(this).find("ul:eq(0)").css({"display":"block"});
	},function(){
	$(this).removeClass("on");
	$(this).find("ul:eq(0)").css({"display":"none"});
	});
$(".float-sidebar .fk").mouseenter(function(event) {
	$(".spit").addClass('on')
});
$(".float-sidebar .fk").mouseleave(function(event) {
	if(!$(".spit textarea").val()){
		$(".spit").removeClass('on')
	}
});
$(".bnt_on").click(function(event) {
	$(".spit").removeClass('on')
	$(".spit textarea").val("")
});   
});
//	回到顶部
function goTop(){
		$('html,body').animate({'scrollTop':0},600);
	}
		