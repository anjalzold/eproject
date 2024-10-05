        // Mock user data for demonstration
        // const users = [
        //     { id: 1, username: 'NarutoFan92', email: 'naruto@example.com', role: 'user', joinedDate: '2023-01-15', avatar: 'https://example.com/avatars/naruto.jpg' },
        //     { id: 2, username: 'SasukeUchiha', email: 'sasuke@example.com', role: 'moderator', joinedDate: '2023-02-20', avatar: 'https://example.com/avatars/sasuke.jpg' },
        //     { id: 3, username: 'AnimeAdmin', email: 'admin@example.com', role: 'admin', joinedDate: '2022-12-01', avatar: 'https://example.com/avatars/admin.jpg' },
        //     // Add more mock users as needed
        // ];

        // const itemsPerPage = 10;
        // let currentPage = 1;

        // function displayUsers(users, page = 1) {
        //     const startIndex = (page - 1) * itemsPerPage;
        //     const endIndex = startIndex + itemsPerPage;
        //     const paginatedUsers = users.slice(startIndex, endIndex);

        //     const tableBody = document.getElementById('userTableBody');
        //     tableBody.innerHTML = '';

        //     paginatedUsers.forEach(user => {
        //         const row = document.createElement('tr');
        //         row.innerHTML = `
        //             <td><img src="${user.avatar}" alt="${user.username}" class="avatar-sm"></td>
        //             <td>${user.username}</td>
        //             <td>${user.email}</td>
        //             <td><span class="badge badge-role badge-${user.role}">${user.role}</span></td>
        //             <td>${user.joinedDate}</td>
        //             <td>
        //                 <button class="btn btn-sm btn-outline-primary" onclick="editUser(${user.id})">Edit</button>
        //                 <button class="btn btn-sm btn-outline-danger" onclick="deleteUser(${user.id})">Delete</button>
        //             </td>
        //         `;
        //         tableBody.appendChild(row);
        //     });

        //     updatePagination(users.length, page);
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

        function changePage(page) {
            currentPage = page;
            displayUsers(users, page);
        }

        function searchUsers() {
            const searchTerm = document.getElementById('searchInput').value.toLowerCase();
            const roleFilter = document.getElementById('roleFilter').value;
            const filteredUsers = users.filter(user =>
                (user.username.toLowerCase().includes(searchTerm) || user.email.toLowerCase().includes(searchTerm)) &&
                (roleFilter === '' || user.role === roleFilter)
            );
            displayUsers(filteredUsers);
        }

        function editUser(userId) {
            // Implement edit user functionality
            console.log(`Edit user with ID: ${userId}`);
        }

        function deleteUser(userId) {
            // Implement delete user functionality
            console.log(`Delete user with ID: ${userId}`);
        }

        // Event listeners
        document.getElementById('searchInput').addEventListener('input', searchUsers);
        document.getElementById('roleFilter').addEventListener('change', searchUsers);
        document.getElementById('addUserBtn').addEventListener('click', () => {
            // Implement navigation to Add User page
            console.log('Navigate to Add User page');
        });

        // Initial display
        displayUsers(users);
