document.addEventListener("DOMContentLoaded", () => {
    loadCatalogItem();
});

function openCreateModal() {
    document.getElementById("modalTitle").innerText = "Create Catalog Item";

    fetch("/CatalogItem/CreatePartial")
        .then(res => res.text())
        .then(html => {
            document.getElementById("modalBody").innerHTML = html;

            attachEventFormListener();

            new bootstrap.Modal(document.getElementById("catalogItemModal")).show();
        });
}

function openEditModal(id) {
    document.getElementById("modalTitle").innerText = "Edit Catalog Item";

    fetch(`/CatalogItem/EditPartial/${id}`)
        .then(res => res.text())
        .then(html => {
            document.getElementById("modalBody").innerHTML = html;

            attachEventFormListener();

            new bootstrap.Modal(document.getElementById("catalogItemModal")).show();
        });
}

async function loadCatalogItem() {
    const tableBody = document.getElementById("catalogItemBody");

    try {
        const response = await fetch("/CatalogItem/GetCatalogItem");

        if (!response.ok) {
            tableBody.innerHTML = `
                    <tr><td colspan="7">Failed to load (Error ${response.status})</td></tr>
                `;
            return;
        }

        const result = await response.json();

        console.log("API result:", result);

        const catalogItems = Array.isArray(result)
            ? result
            : result.data ?? result.catalogItems ?? [];


        if (!catalogItems || catalogItems.length === 0) {
            tableBody.innerHTML = `
                    <tr><td colspan="7">No catalog item found.</td></tr>
                `;
            return;
        }

        tableBody.innerHTML = "";

        catalogItems.forEach(ci => {
            //<td>${formatDate(rf.date)}</td>
            const row = `
                    <tr>
                        <td>${ci.name}</td>
                        <td>${ci.description}</td>
                        <td>${ci.price}</td>
                        <td>
                            <button class="btn btn-sm btn-primary" onclick="openEditModal('${ci.id}')">Edit</button>
                        </td>
                        <td>
                            <button class="btn btn-sm btn-danger" onclick="deleteCatalogItem('${ci.id}')">Delete</button>
                        </td>
                    </tr>
                `;
            tableBody.innerHTML += row;
        });

    } catch (error) {
        console.error("Error loading catalog items:", error);
        tableBody.innerHTML = `
                <tr><td colspan="7">Unexpected error while loading catalog items.</td></tr>
            `;
    }
}

function attachEventFormListener() {
    const form = document.getElementById("catalogItemForm");
    if (!form) {

        return;
    }


    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        console.log("submit fired");

        const payload = {
            id: document.getElementById('Id').value || null,
            name: document.getElementById('Name').value,
            description: document.getElementById('Description').value,
            price: document.getElementById('Price').value,
        };


        const isUpdate = payload.id && payload.id !== "";
        const url = isUpdate ? '/CatalogItem/Update/' + payload.id : '/CatalogItem/Create';

        const response = await fetch(url, {
            method: isUpdate ? 'PUT' : 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        if (result.success) {
            alert('Saved successfully!');
            setTimeout(() => location.reload(), 500);
        } else {
            alert(result.message);
        }
    });
}

async function deleteCatalogItem(id) {
    if (!confirm("Are you sure you want to delete this catalog item?")) return;

    const response = await fetch(`/CatalogItem/Delete/${id}`, {
        method: "DELETE"
    });

    const result = await response.json();

    if (result.success) {
        alert("Deleted successfully!");
        loadCatalogItem();
    } else {
        alert(result.message || "Delete failed");
    }
}