function confirmBox(msgtext) {
    let text = (msgtext);
    if (confirm(text) == true) {
        return true;
    }
    return false;
}
function alertData(msg) {
    return window.alert(msg);
}