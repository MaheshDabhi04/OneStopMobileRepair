// ============================================================
//  ONESTOP MOBILE REPAIR — site.js
//  Place in wwwroot/js/site.js
// ============================================================

document.addEventListener('DOMContentLoaded', function () {
    // Mark body so CSS knows JS loaded — enables scroll reveal animations
    document.body.classList.add('js-loaded');

    // ── CUSTOM CURSOR ────────────────────────────────────────
    const cursor = document.getElementById('cursor');
    const ring = document.getElementById('cursorRing');
    if (cursor && ring) {
        let mx = 0, my = 0, rx = 0, ry = 0;
        document.addEventListener('mousemove', function (e) {
            mx = e.clientX; my = e.clientY;
            cursor.style.left = mx + 'px';
            cursor.style.top = my + 'px';
        });
        (function animRing() {
            rx += (mx - rx) * 0.13;
            ry += (my - ry) * 0.13;
            ring.style.left = rx + 'px';
            ring.style.top = ry + 'px';
            requestAnimationFrame(animRing);
        })();
        document.querySelectorAll('a, button, .service-card, .stat-card, .contact-card, .social-card, .why-card, .tech-card').forEach(function (el) {
            el.addEventListener('mouseenter', function () {
                cursor.style.transform = 'translate(-50%,-50%) scale(2.2)';
                cursor.style.background = 'var(--neon2)';
                ring.style.width = '60px';
                ring.style.height = '60px';
            });
            el.addEventListener('mouseleave', function () {
                cursor.style.transform = 'translate(-50%,-50%) scale(1)';
                cursor.style.background = 'var(--neon)';
                ring.style.width = '38px';
                ring.style.height = '38px';
            });
        });
    }

    // ── SCROLL PROGRESS BAR ──────────────────────────────────
    const bar = document.getElementById('progressBar');
    if (bar) {
        window.addEventListener('scroll', function () {
            var pct = window.scrollY / (document.body.scrollHeight - window.innerHeight) * 100;
            bar.style.width = pct + '%';
        }, { passive: true }); /* passive=true = no scroll jank */
    }

    // ── SCROLL REVEAL ────────────────────────────────────────
    var reveals = document.querySelectorAll('.reveal');
    var obs = new IntersectionObserver(function (entries) {
        entries.forEach(function (e) {
            if (e.isIntersecting) e.target.classList.add('visible');
        });
    }, { threshold: 0.12 });
    reveals.forEach(function (r) { obs.observe(r); });

    // ── FLOATING PARTICLES ───────────────────────────────────
    var pContainer = document.getElementById('particles');
    if (pContainer) {
        for (var i = 0; i < 28; i++) {
            var p = document.createElement('div');
            p.className = 'particle';
            p.style.left = (Math.random() * 100) + 'vw';
            p.style.animationDuration = (8 + Math.random() * 14) + 's';
            p.style.animationDelay = (Math.random() * 12) + 's';
            var size = (1.5 + Math.random() * 2.5) + 'px';
            p.style.width = size;
            p.style.height = size;
            p.style.background = Math.random() > 0.5 ? 'var(--neon)' : 'var(--neon2)';
            pContainer.appendChild(p);
        }
    }

    // ── ACTIVE NAV LINK ──────────────────────────────────────
    var currentPage = window.location.pathname.split('/').pop().toLowerCase();
    document.querySelectorAll('.nav-links a').forEach(function (link) {
        var href = (link.getAttribute('href') || '').toLowerCase();
        if (
            (currentPage === '' || currentPage === 'home' || currentPage === 'index') && (href.includes('home') || href === '/' || href === '') ||
            currentPage.includes('about') && href.includes('about') ||
            currentPage.includes('contact') && href.includes('contact')
        ) {
            link.classList.add('active');
        }
    });

});

// ── FAQ ACCORDION (used on Contact page) ─────────────────────
function toggleFAQ(btn) {
    var answer = btn.nextElementSibling;
    var isOpen = btn.classList.contains('open');
    document.querySelectorAll('.faq-q').forEach(function (b) {
        b.classList.remove('open');
        b.nextElementSibling.classList.remove('open');
    });
    if (!isOpen) {
        btn.classList.add('open');
        answer.classList.add('open');
    }
}

// ── CONTACT FORM SUCCESS (Contact page) ──────────────────────
function handleSubmit() {
    const formData = new FormData();
    formData.append("Name", $("#txtName").val());
    formData.append("Phone", $("#txtPhone").val());
    formData.append("Service", $("#ddlService").val());
    formData.append("Device", $("#txtDevice").val());
    formData.append("Message", $("#txtMessage").val());

    const frontInput = document.getElementById('fileFrontImage');
    if (frontInput && frontInput.files.length > 0) {
        formData.append("FrontImageFile", frontInput.files[0]);
    }

    const backInput = document.getElementById('fileBackImage');
    if (backInput && backInput.files.length > 0) {
        formData.append("BackImageFile", backInput.files[0]);
    }

    if (!formData.get("Name") || !formData.get("Phone")) {
        alert("Please enter both Name and Phone number.");
        return;
    }

    $.ajax({
        url: "/Website/Contact",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        success: function (res) {
            if (res.success) {
                document.getElementById('formBody').style.display = 'none';
                document.getElementById('formSuccess').style.display = 'block';
            } else {
                alert("Error: " + (res.message || "Something went wrong"));
            }
        },
        error: function () {
            alert("An error occurred. Please try again.");
        }
    });
}
