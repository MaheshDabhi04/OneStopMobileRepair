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

// ── ENQUIRY FORM VALIDATION + SUBMIT ─────────────────────────
function _enqErr(fieldId, errId, hasError) {
    var field = document.getElementById(fieldId);
    var err   = document.getElementById(errId);
    if (field) {
        field.classList.toggle('inp-err', hasError);
        field.classList.toggle('inp-ok',  !hasError);
    }
    if (err) err.classList.toggle('show', hasError);
    return !hasError;
}

function handleSubmit() {
    var name    = (document.getElementById('txtName')   || {}).value || '';
    var phone   = (document.getElementById('txtPhone')  || {}).value || '';
    var service = (document.getElementById('ddlService')|| {}).value || '';
    var device  = (document.getElementById('txtDevice') || {}).value || '';

    var nameOk    = _enqErr('txtName',    'err-name',    !name.trim()   || !/^[a-zA-Z\s]+$/.test(name.trim()));
    var phoneOk   = _enqErr('txtPhone',   'err-phone',   !/^\d{10}$/.test(phone.trim()));
    var serviceOk = _enqErr('ddlService', 'err-service', !service);
    var deviceOk  = _enqErr('txtDevice',  'err-device',  !device.trim());

    if (!nameOk || !phoneOk || !serviceOk || !deviceOk) {
        // Scroll to first error
        var first = document.querySelector('.inp-err');
        if (first) first.scrollIntoView({ behavior: 'smooth', block: 'center' });
        return;
    }

    // ── Build FormData and POST ───────────────────────────────
    var formData = new FormData();
    formData.append("Name",    name.trim());
    formData.append("Phone",   phone.trim());
    formData.append("Service", service);
    formData.append("Device",  device.trim());

    var msg = document.getElementById('txtMessage');
    if (msg) formData.append("Message", msg.value);

    var frontInput = document.getElementById('fileFrontImage');
    if (frontInput && frontInput.files.length > 0)
        formData.append("FrontImageFile", frontInput.files[0]);

    var backInput = document.getElementById('fileBackImage');
    if (backInput && backInput.files.length > 0)
        formData.append("BackImageFile", backInput.files[0]);

    // Disable button to prevent double-submit
    var btn = document.querySelector('.submit-btn');
    if (btn) { btn.disabled = true; btn.textContent = 'Sending...'; }

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
                if (btn) { btn.disabled = false; btn.textContent = 'SEND ENQUIRY →'; }
            }
        },
        error: function () {
            alert("An error occurred. Please try again.");
            if (btn) { btn.disabled = false; btn.textContent = 'SEND ENQUIRY →'; }
        }
    });
}

// ── Live listeners for enquiry form ──────────────────────────
document.addEventListener('DOMContentLoaded', function () {
    var txtName = document.getElementById('txtName');
    var txtPhone = document.getElementById('txtPhone');
    var ddlService = document.getElementById('ddlService');
    var txtDevice = document.getElementById('txtDevice');

    if (txtName)    txtName.addEventListener('input', function() { _enqErr('txtName','err-name', !this.value.trim() || !/^[a-zA-Z\s]+$/.test(this.value.trim())); });
    if (txtPhone)   txtPhone.addEventListener('input', function() { this.value = this.value.replace(/\D/g,'').slice(0,10); _enqErr('txtPhone','err-phone', !/^\d{10}$/.test(this.value)); });
    if (ddlService) ddlService.addEventListener('change', function() { _enqErr('ddlService','err-service', !this.value); });
    if (txtDevice)  txtDevice.addEventListener('input', function() { _enqErr('txtDevice','err-device', !this.value.trim()); });
});
