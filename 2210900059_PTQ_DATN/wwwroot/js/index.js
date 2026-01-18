// Simple slider with dots
(function () {
    const slider = document.getElementById('slider');
    const slides = slider.querySelectorAll('.slide');
    const dotsWrap = document.getElementById('dots');

    let current = 0;
    const total = slides.length;
    let timer = null;
    // create dots
    for (let i = 0; i < total; i++) {
        const btn = document.createElement('button');
        btn.dataset.index = i;
        btn.addEventListener('click', (e) => {
            goTo(parseInt(e.currentTarget.dataset.index));
            resetTimer();
        });
        dotsWrap.appendChild(btn);
    }
    const dots = dotsWrap.querySelectorAll('button');
    function update() {
        slider.style.transform = `translateX(-${current * 100}%)`;
        dots.forEach(d => d.classList.remove('active'));
        if (dots[current]) dots[current].classList.add('active');
    }
    function goTo(i) {
        current = (i + total) % total;
        update();
    }
    function next() {
        goTo(current + 1);
    }
    function resetTimer() {
        if (timer) clearInterval(timer);
        timer = setInterval(next, 5000);
    }
    // init
    update();
    resetTimer();

    // pause on hover
    slider.addEventListener('mouseenter', () => clearInterval(timer));
    slider.addEventListener('mouseleave', resetTimer);

})();
