// dashboard.js

// document.addEventListener("DOMContentLoaded", function () {
//   const ctx = document.getElementById("animePopularityChart");
//   if (ctx) {
//     new Chart(ctx, {
//       type: "bar",
//       data: {
//         labels: [
//           "Naruto",
//           "One Piece",
//           "Attack on Titan",
//           "My Hero Academia",
//           "Demon Slayer",
//         ],
//         datasets: [
//           {
//             label: "Popularity Score",
//             data: [95, 98, 92, 88, 90],
//             backgroundColor: [
//               "rgba(255, 99, 132, 0.8)",
//               "rgba(54, 162, 235, 0.8)",
//               "rgba(255, 206, 86, 0.8)",
//               "rgba(75, 192, 192, 0.8)",
//               "rgba(153, 102, 255, 0.8)",
//             ],
//             borderColor: [
//               "rgba(255, 99, 132, 1)",
//               "rgba(54, 162, 235, 1)",
//               "rgba(255, 206, 86, 1)",
//               "rgba(75, 192, 192, 1)",
//               "rgba(153, 102, 255, 1)",
//             ],
//             borderWidth: 1,
//           },
//         ],
//       },
//       options: {
//         scales: {
//           y: {
//             beginAtZero: true,
//             max: 100,
//           },
//         },
//         plugins: {
//           legend: {
//             display: false,
//           },
//         },
//       },
//     });
//   }
// });

     // Avatar preview functionality
    

        // Image preview functionality
        document
          .getElementById("coverImage")
          .addEventListener("change", function (e) {
            const file = e.target.files[0];
            if (file) {
              const reader = new FileReader();
              reader.onload = function (e) {
                document.getElementById("coverPreview").src = e.target.result;
              };
              reader.readAsDataURL(file);
            }
          });



        // Mock anime data for demonstration
        // const animeList = [
        //     { id: 1, title: 'Naruto', genres: ['Action', 'Adventure'], rating: 4.5, releaseDate: '2002-10-03', poster: 'https://example.com/posters/naruto.jpg' },
        //     { id: 2, title: 'Death Note', genres: ['Mystery', 'Thriller'], rating: 4.7, releaseDate: '2006-10-03', poster: 'https://example.com/posters/deathnote.jpg' },
        //     { id: 3, title: 'Attack on Titan', genres: ['Action', 'Dark Fantasy'], rating: 4.8, releaseDate: '2013-04-07', poster: 'https://example.com/posters/aot.jpg' },
        //     // Add more mock anime data as needed
        // ];

        // const itemsPerPage = 10;
        // let currentPage = 1;

        // function displayAnime(animeList, page = 1) {
        //     const startIndex = (page - 1) * itemsPerPage;
        //     const endIndex = startIndex + itemsPerPage;
        //     const paginatedAnime = animeList.slice(startIndex, endIndex);

        //     const tableBody = document.getElementById('animeTableBody');
        //     tableBody.innerHTML = '';

        //     paginatedAnime.forEach(anime => {
        //         const row = document.createElement('tr');
        //         row.innerHTML = `
        //             <td><img src="${anime.poster}" alt="${anime.title}" class="anime-poster"></td>
        //             <td>${anime.title}</td>
        //             <td>${anime.genres.map(genre => `<span class="badge badge-genre">${genre}</span>`).join(' ')}</td>
        //             <td>
        //                 <span class="star-rating">
        //                     ${'★'.repeat(Math.floor(anime.rating))}${'☆'.repeat(5 - Math.floor(anime.rating))}
        //                 </span>
        //                 ${anime.rating.toFixed(1)}
        //             </td>
        //             <td>${anime.releaseDate}</td>
        //             <td>
        //                 <button class="btn btn-sm btn-outline-primary" onclick="editAnime(${anime.id})">Edit</button>
        //                 <button class="btn btn-sm btn-outline-danger" onclick="deleteAnime(${anime.id})">Delete</button>
        //             </td>
        //         `;
        //         tableBody.appendChild(row);
        //     });

        //     updatePagination(animeList.length, page);
        // }

        // function updatePagination(totalItems, currentPage) {
        //     const totalPages = Math.ceil(totalItems / itemsPerPage);
        //     const pagination = document.getElementById('pagination');
        //     pagination.innerHTML = '';

        //     for (let i = 1; i <= totalPages; i++) {
        //         const li = document.createElement('li');
        //         li.classList.add('page-item');
        //         if (i === currentPage) li.classList.add('active');
        //         li.innerHTML = `<a class="page-link" href="#" onclick="changePage(${i})">${i}</a>`;
        //         pagination.appendChild(li);
        //     }
        // }

        // function changePage(page) {
        //     currentPage = page;
        //     displayAnime(animeList, page);
        // }

        // function searchAndFilterAnime() {
        //     const searchTerm = document.getElementById('searchInput').value.toLowerCase();
        //     const genreFilter = document.getElementById('genreFilter').value.toLowerCase();
        //     const sortBy = document.getElementById('sortBy').value;

        //     let filteredAnime = animeList.filter(anime =>
        //         anime.title.toLowerCase().includes(searchTerm) &&
        //         (genreFilter === '' || anime.genres.some(genre => genre.toLowerCase() === genreFilter))
        //     );

        //     filteredAnime.sort((a, b) => {
        //         if (sortBy === 'title') return a.title.localeCompare(b.title);
        //         if (sortBy === 'rating') return b.rating - a.rating;
        //         if (sortBy === 'releaseDate') return new Date(b.releaseDate) - new Date(a.releaseDate);
        //         return 0;
        //     });

        //     displayAnime(filteredAnime);
        // }

        // function editAnime(animeId) {
        //     // Implement edit anime functionality
        //     console.log(`Edit anime with ID: ${animeId}`);
        // }

        // function deleteAnime(animeId) {
        //     // Implement delete anime functionality
        //     console.log(`Delete anime with ID: ${animeId}`);
        // }

        // Event listeners
        // document.getElementById('searchInput').addEventListener('input', searchAndFilterAnime);
        // document.getElementById('genreFilter').addEventListener('change', searchAndFilterAnime);
        // document.getElementById('sortBy').addEventListener('change', searchAndFilterAnime);
        // document.getElementById('addAnimeBtn').addEventListener('click', () => {
        //     // Implement navigation to Add Anime page
        //     console.log('Navigate to Add Anime page');
        // });

        // Initial display
        // displayAnime(animeList);

     

        // // Form submission
        // document.getElementById('addAnimeForm').addEventListener('submit', function (e) {
        //     e.preventDefault();

        //     // Gather form data
        //     const formData = new FormData();
        //     formData.append('title', document.getElementById('animeTitle').value);
        //     formData.append('genre', Array.from(document.getElementById('animeGenre').selectedOptions).map(option => option.value));
        //     formData.append('releaseDate', document.getElementById('animeReleaseDate').value);
        //     formData.append('episodes', document.getElementById('animeEpisodes').value);
        //     formData.append('synopsis', document.getElementById('animeSynopsis').value);
        //     formData.append('cover', document.getElementById('animeCover').files[0]);

        //     // Here you would typically send the formData to your backend API
        //     console.log('Form submitted', Object.fromEntries(formData));

        //     // Reset form after submission
        //     this.reset();
        //     document.getElementById('coverPreview').src = 'https://via.placeholder.com/300x400';

        //     // Show success message (you can replace this with a more sophisticated notification)
        //     alert('Anime added successfully!');
        // });
