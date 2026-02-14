<script>
        // Define data at the beginning
    const documents = [
    {
        "id": 1,
    "title": "Machine Learning in Healthcare: A Comprehensive Review",
    "author": "Dr. Sarah Johnson",
    "date": "2024-03-15",
    "type": "pdf",
    "size": "4.2 MB",
    "downloads": 142,
    "category": "Research Paper",
    "description": "An extensive review of machine learning applications in modern healthcare systems."
            },
    {
        "id": 2,
    "title": "Climate Change Impact Analysis 2024",
    "author": "Environmental Research Team",
    "date": "2024-03-10",
    "type": "doc",
    "size": "2.8 MB",
    "downloads": 89,
    "category": "Technical Report",
    "description": "Detailed analysis of climate change impacts across different regions."
            },
    {
        "id": 3,
    "title": "Quantum Computing Fundamentals",
    "author": "Prof. Michael Chen",
    "date": "2024-03-05",
    "type": "ppt",
    "size": "5.1 MB",
    "downloads": 203,
    "category": "Educational Material",
    "description": "Introduction to quantum computing concepts and applications."
            },
    {
        "id": 4,
    "title": "Blockchain Technology in Finance",
    "author": "Financial Innovations Lab",
    "date": "2024-02-28",
    "type": "pdf",
    "size": "3.7 MB",
    "downloads": 167,
    "category": "Case Study",
    "description": "Case study on blockchain implementation in financial institutions."
            },
    {
        "id": 5,
    "title": "Renewable Energy Storage Solutions",
    "author": "Energy Research Institute",
    "date": "2024-02-20",
    "type": "doc",
    "size": "2.3 MB",
    "downloads": 94,
    "category": "Research Paper",
    "description": "Analysis of modern energy storage technologies for renewable sources."
            },
    {
        "id": 6,
    "title": "AI Ethics Framework Proposal",
    "author": "Ethics in Tech Committee",
    "date": "2024-02-15",
    "type": "txt",
    "size": "1.1 MB",
    "downloads": 78,
    "category": "Policy Document",
    "description": "Proposed ethical framework for AI development and deployment."
            }
    ];

    const blogPosts = [
    {
        "id": 1,
    "title": "The Future of AI in Scientific Research",
    "author": "Alexandra Rivera",
    "date": "2 days ago",
    "readTime": "8 min read",
    "likes": 42,
    "comments": 12,
    "excerpt": "Exploring how artificial intelligence is revolutionizing the way we conduct and analyze scientific research across various disciplines."
            },
    {
        "id": 2,
    "title": "Open Source Research: Benefits and Challenges",
    "author": "David Park",
    "date": "1 week ago",
    "readTime": "6 min read",
    "likes": 31,
    "comments": 8,
    "excerpt": "A deep dive into the open source research movement and its impact on scientific collaboration and innovation."
            },
    {
        "id": 3,
    "title": "Data Visualization Techniques for Researchers",
    "author": "Maria Gonzalez",
    "date": "2 weeks ago",
    "readTime": "10 min read",
    "likes": 56,
    "comments": 15,
    "excerpt": "Essential data visualization methods that every researcher should know to effectively communicate their findings."
            }
    ];

    const categories = [
    "Research Paper", "Case Study", "Technical Report", "Literature Review",
    "Methodology", "Policy Document", "Educational Material", "Dataset"
    ];

    // Function to get icon class based on document type
    function getDocumentIcon(type) {
            const icons = {
        "pdf": {"class": "pdf-icon", "icon": "fa-file-pdf" },
    "doc": {"class": "doc-icon", "icon": "fa-file-word" },
    "ppt": {"class": "ppt-icon", "icon": "fa-file-powerpoint" },
    "txt": {"class": "txt-icon", "icon": "fa-file-alt" }
            };
    return icons[type] || {"class": "txt-icon", "icon": "fa-file" };
        }

    // Function to format date
    function formatDate(dateString) {
            const date = new Date(dateString);
    return date.toLocaleDateString("en-US", {
        "year": "numeric",
    "month": "short",
    "day": "numeric"
            });
        }

    // Function to render document cards
    function renderDocuments() {
            const container = document.getElementById("documentsGrid");
    container.innerHTML = "";

            documents.forEach(doc => {
                const icon = getDocumentIcon(doc.type);
    const card = document.createElement("div");
    card.className = "bg-white rounded-xl shadow-sm border border-gray-200 p-5 card-hover";
    card.innerHTML = `
    <div class="flex items-start justify-between mb-4">
        <div class="document-icon ${icon.class}">
            <i class="fas ${icon.icon} text-xl"></i>
        </div>
        <span class="text-xs font-medium px-2 py-1 rounded-full bg-gray-100 text-gray-700">${doc.category}</span>
    </div>

    <h3 class="font-bold text-dark text-lg mb-2 line-clamp-2">${doc.title}</h3>
    <p class="text-gray-600 text-sm mb-4 line-clamp-2">${doc.description}</p>

    <div class="flex items-center justify-between text-sm text-gray-500 mb-4">
        <div class="flex items-center space-x-1">
            <i class="fas fa-user"></i>
            <span>${doc.author}</span>
        </div>
        <div>${formatDate(doc.date)}</div>
    </div>

    <div class="flex items-center justify-between pt-4 border-t">
        <div class="flex items-center space-x-4">
            <div class="flex items-center space-x-1">
                <i class="fas fa-download text-gray-400"></i>
                <span class="text-sm">${doc.downloads}</span>
            </div>
            <div class="flex items-center space-x-1">
                <i class="fas fa-file text-gray-400"></i>
                <span class="text-sm">${doc.size}</span>
            </div>
        </div>
        <button class="text-primary hover:text-secondary font-medium flex items-center space-x-1">
            <i class="fas fa-eye"></i>
            <span>View</span>
        </button>
    </div>
    `;
    container.appendChild(card);
            });
        }

    // Function to render blog posts
    function renderBlogPosts() {
            const container = document.getElementById("blogPostsList");
    container.innerHTML = "";

            blogPosts.forEach(post => {
                const postElement = document.createElement("div");
    postElement.className = "pb-6 border-b border-gray-200 last:border-0 last:pb-0";
    postElement.innerHTML = `
    <h4 class="font-bold text-dark text-lg mb-2 hover:text-primary cursor-pointer">${post.title}</h4>
    <p class="text-gray-600 text-sm mb-3 line-clamp-2">${post.excerpt}</p>

    <div class="flex items-center justify-between text-sm text-gray-500">
        <div class="flex items-center space-x-3">
            <div class="flex items-center space-x-1">
                <i class="fas fa-user-circle"></i>
                <span>${post.author}</span>
            </div>
            <span>${post.date}</span>
        </div>

        <div class="flex items-center space-x-3">
            <div class="flex items-center space-x-1">
                <i class="far fa-heart"></i>
                <span>${post.likes}</span>
            </div>
            <div class="flex items-center space-x-1">
                <i class="far fa-comment"></i>
                <span>${post.comments}</span>
            </div>
        </div>
    </div>
    `;
    container.appendChild(postElement);
            });
        }

    // Function to handle search
    function handleSearch() {
            const searchInput = document.getElementById("searchInput");
    const searchBtn = searchInput.nextElementSibling;

            const performSearch = () => {
                const query = searchInput.value.toLowerCase().trim();
    if (query) {
                    // Filter documents
                    const filteredDocs = documents.filter(doc =>
    doc.title.toLowerCase().includes(query) ||
    doc.author.toLowerCase().includes(query) ||
    doc.description.toLowerCase().includes(query) ||
    doc.category.toLowerCase().includes(query)
    );

                    // Filter blog posts
                    const filteredPosts = blogPosts.filter(post =>
    post.title.toLowerCase().includes(query) ||
    post.author.toLowerCase().includes(query) ||
    post.excerpt.toLowerCase().includes(query)
    );

    // Update UI with filtered results
    console.log("Search results:", {"documents": filteredDocs, "blogPosts": filteredPosts });
    alert(`Found ${filteredDocs.length} documents and ${filteredPosts.length} blog posts matching "${query}"`);
                }
            };

            searchInput.addEventListener("keypress", (e) => {
                if (e.key === "Enter") {
        performSearch();
                }
            });

    searchBtn.addEventListener("click", performSearch);
        }

    // Function to handle file upload
    function handleFileUpload() {
            const fileInput = document.getElementById("fileUpload");
    const uploadBtn = document.getElementById("uploadBtn");

            fileInput.addEventListener("change", (e) => {
                const files = Array.from(e.target.files);
                if (files.length > 0) {
        console.log("Files selected for upload:", files);
    // In a real application, you would upload files to a server here
    alert(`${files.length} file(s) selected for upload`);
                }
            });

            uploadBtn.addEventListener("click", () => {
        fileInput.click();
            });
        }

    // Function to handle new blog post creation
    function handleNewBlogPost() {
            const newPostBtn = document.getElementById("newPostBtn");

            newPostBtn.addEventListener("click", () => {
        // Scroll to blog post creation section
        document.querySelector(".lg\\:w-1\\/3").scrollIntoView({
            "behavior": "smooth"
        });

    // Focus on title input
    const titleInput = document.querySelector(".lg\\:w-1\\/3 input[type='text']");
    if (titleInput) {
        titleInput.focus();
                }
            });
        }

    // Function to initialize modals
    function initModals() {
            const uploadModal = document.getElementById("uploadModal");
    const closeUploadModal = document.getElementById("closeUploadModal");

            // Close modal when clicking outside
            uploadModal.addEventListener("click", (e) => {
                if (e.target === uploadModal) {
        uploadModal.classList.add("hidden");
                }
            });

            closeUploadModal.addEventListener("click", () => {
        uploadModal.classList.add("hidden");
            });
        }

    // Function to prevent default anchor behavior
    function preventAnchorDefaults() {
        document.addEventListener("click", (e) => {
            if (e.target.tagName === "A") {
                e.preventDefault();
                const href = e.target.getAttribute("href");
                if (href === "#") {
                    // Handle internal navigation
                    console.log("Internal navigation to:", href);
                }
            }
        });
        }

        // Initialize everything when DOM is loaded
        document.addEventListener("DOMContentLoaded", () => {
        renderDocuments();
    renderBlogPosts();
    handleSearch();
    handleFileUpload();
    handleNewBlogPost();
    initModals();
    preventAnchorDefaults();

    // Add some sample images to the page
    const images = document.querySelectorAll("img[src*='picsum.photos']");
            images.forEach((img, index) => {
        img.src = `https://picsum.photos/${img.width || 400}/${img.height || 300}?random=${index + 1}`;
    img.loading = "lazy";
            });
        });
</script>