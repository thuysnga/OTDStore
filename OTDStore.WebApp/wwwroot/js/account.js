const accNav = document.querySelectorAll(".account__nav--profile");
const accEdit = document.querySelector(".account__nav--edit");
accEdit.addEventListener("click", () => {
    console.log("hello");
});

function activeSidebar() {
    accNav.forEach(item => item.classList.remove("acActive"));
    this.classList.add("acActive");
}
accNav.forEach(a => a.addEventListener("click", activeSidebar));