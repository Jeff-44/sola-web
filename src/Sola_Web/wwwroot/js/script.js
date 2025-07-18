
        // JavaScript pour le site

    // Menu mobile
    const mobileMenuBtn = document.querySelector('.mobile-menu-btn');
    const navLinks = document.querySelector('.nav-links');
        
        mobileMenuBtn.addEventListener('click', () => {
        navLinks.style.display = navLinks.style.display === 'flex' ? 'none' : 'flex';
        });

    // Animation au défilement
    const fadeElements = document.querySelectorAll('.fade-in');
        
        const appearOnScroll = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = 1;
                appearOnScroll.unobserve(entry.target);
            }
        });
        }, {threshold: 0.1 });
        
        fadeElements.forEach(element => {
        element.style.opacity = 0;
    appearOnScroll.observe(element);
        });

    // Gestion du formulaire
    const orderForm = document.getElementById('orderForm');

    if (orderForm) {
        orderForm.addEventListener('submit', (e) => {
        e.preventDefault();

    // Récupération des données du formulaire
    const formData = {
        firstName: document.getElementById('firstName').value,
    lastName: document.getElementById('lastName').value,
    email: document.getElementById('email').value,
    phone: document.getElementById('phone').value,
    product: document.getElementById('product').value,
    quantity: document.getElementById('quantity').value,
    address: document.getElementById('address').value,
    message: document.getElementById('message').value
            };

    // Ici vous pouvez envoyer les données à votre backend
    console.log('Données du formulaire:', formData);

    // Simulation d'envoi
    alert('Merci pour votre commande! Nous vous contacterons sous peu pour finaliser les détails.');
        orderForm.reset();

            // Redirection (optionnelle)
            // window.location.href = 'merci.html';
        });
    }

    // Calculatrice solaire (exemple simple)
    function calculateSavings() {
            const consumption = parseFloat(document.getElementById('consumption').value);
    if (!consumption) return;

    const estimatedPanels = Math.ceil(consumption / 350);
    const estimatedCost = estimatedPanels * 250;
    const annualSavings = consumption * 0.15;

    document.getElementById('result').innerHTML = `
    <h3>Résultats:</h3>
    <p><strong>Panneaux estimés:</strong> ${estimatedPanels} (350W chacun)</p>
    <p><strong>Investissement estimé:</strong> $${estimatedCost.toLocaleString()}</p>
    <p><strong>Économies annuelles estimées:</strong> $${annualSavings.toLocaleString()}</p>
    <p><strong>Retour sur investissement:</strong> ${Math.round(estimatedCost / annualSavings)} ans</p>
    `;
        }
