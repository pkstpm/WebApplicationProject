// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//main part
function changeImage(num) {
    const image = document.getElementById("img-section2");
    const texts = ["AEROBIC", "BASKETBALL", "BADMINTON", "BICYCLE", "FOOTBALL", "VOLLEYBALL", "FUTSAL", "SKATEBOARD", "SWIMMING", "OTHER"];

    if (num == 1) {
        image.src = "https://img.hotimg.com/aerobic-woman-shadow.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }
    }
    if (num == 2) {
        image.src = "https://img.hotimg.com/basketball-man-shadow.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }
    }
    if (num == 3) {
        image.src = "https://img.hotimg.com/badminton-man-standing-shadow.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }
    }
    if (num == 4) {
        image.src = "https://img.hotimg.com/new_bicycle.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }
    }
    if (num == 5) {
        image.src = "https://img.hotimg.com/fbplayer.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }
    }
    if (num == 6) {
        image.src = "https://img.hotimg.com/new-volleyball.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }

    }
    if (num == 7) {
        image.src = "https://img.hotimg.com/ben-weber-r-krWscXjvQ-unsplash-removebg-preview.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }

    }
    if (num == 8) {
        image.src = "https://img.hotimg.com/game-sport-culture-activity-street-removebg-preview.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }

    }
    if (num == 9) {
        image.src = "https://img.hotimg.com/mid-shot-swimmer-stretching-out-removebg-preview.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }

    }
    if (num == 10) {
        image.src = "https://img.hotimg.com/Pngtreevector-question-mark-icon_3989724.png";
        for (let i = 0; i < 10; i++) {
            document.getElementById(`text-behind${i + 1}`).innerText = texts[num - 1];
        }

    }

}

//login part
function showPassword() {
    let x = document.getElementById("eye-icon");
    console.dir(x);
    x.type = "text";
}
function hidePassword() {
    let x = document.getElementById("eye-icon");
    x.type = "password";
}

//createEvent part
const add = document.getElementById("btn-up");
const minus = document.getElementById("btn-down");
const count = document.getElementById("people-count");

add.addEventListener("click", function () {
    count.value = parseInt(count.value) + 1; // Increment the value of the input field
});

minus.addEventListener("click", function () {
    const currentValue = parseInt(count.value);
    if (currentValue > 1) {
        count.value = currentValue - 1; // Decrement the value of the input field, ensuring it never goes below 1
    }
});