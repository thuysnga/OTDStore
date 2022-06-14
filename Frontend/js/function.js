//Range slide price
const range = document.querySelector("#input-range");
const valueRight = document.querySelector(".right");
range.oninput = (() => {
    let value = range.value;
    console.log(value);
    valueRight.textContent = value;
});

//nav filter toggle
const filter = document.querySelector(".typeProduct-filter");
const nav = document.querySelector(".nav-filter");
const navBtn = document.querySelector(".fa-times");
navBtn.addEventListener("click", () => {
    nav.classList.toggle("nav-filter-open");
});
filter.addEventListener("click", () => {
    nav.classList.toggle("nav-filter-open");
});

//nav fitler option
// const optionbtn = document.querySelector(".fa-plus");
// const optionTable = document.querySelector(".nav-filter-select");
// optionbtn.addEventListener("click", () => {
//     optionTable.classList.toggle("nav-filter-select-open");
// })