let id = (id) => document.getElementById(id);

let classes = (classes) => document.getElementsByClassName(classes);

let to= id("to"),
  ActualCost = id("ActualCost"),
  DescriptionOfGiftGiven= id("DescriptionOfGiftGiven"),
  form = id("form"),
  errorMsg = classes("error"),
  successIcon = classes("success-icon"),
  failureIcon = classes("failure-icon");

form.addEventListener("SaveGiftRecord", (e) => {
  e.preventDefault();

  engine(to, 0, "Username cannot be blank");
  engine(ActualCost, 1, "Email cannot be blank");
  engine(DescriptionOfGiftGiven, 2, "Password cannot be blank");
});

let engine = (id, serial, message) => {
  if (id.value.trim() === "") {
    errorMsg[serial].innerHTML = message;
    id.style.border = "2px solid red";

    // icons
    failureIcon[serial].style.opacity = "1";
    successIcon[serial].style.opacity = "0";
  } else {
    errorMsg[serial].innerHTML = "";
    id.style.border = "2px solid green";

    // icons
    failureIcon[serial].style.opacity = "0";
    successIcon[serial].style.opacity = "1";
  }
};